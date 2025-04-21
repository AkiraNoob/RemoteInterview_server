import { Socket } from "socket.io";
import { MediasoupService } from "../services/mediasoup-service";
import { RoomService } from "../services/room-service";
import { ISocketPayloads, IUser, MediaKind } from "../types";

export class SocketHandler {
  private roomService: RoomService;
  private mediasoupService: MediasoupService;

  constructor(roomService: RoomService, mediasoupService: MediasoupService) {
    this.roomService = roomService;
    this.mediasoupService = mediasoupService;
  }

  handleConnection(socket: Socket): void {
    console.log(`Client connected: ${socket.id}`);

    // Handle room join
    socket.on(
      "joinRoom",
      async (payload: ISocketPayloads["joinRoom"], callback) => {
        try {
          const { roomId, userId, name } = payload;
          let room = this.roomService.getRoom(roomId);

          // Create room if it doesn't exist
          if (!room) {
            room = await this.roomService.createRoom(`Room-${roomId}`);
          }

          // Create user
          const user: IUser = {
            id: userId,
            name,
            socket,
            transports: new Map(),
            producers: new Map(),
            consumers: new Map(),
          };

          // Add user to room
          room.addUser(user);

          // Join socket.io room
          socket.join(roomId);

          // Notify other users in room about new user
          socket.to(roomId).emit("userJoined", {
            userId,
            name,
          });

          // Send list of existing users to the new user
          const roomUsers = room
            .getUsers()
            .filter((u) => u.id !== userId)
            .map((u) => ({ id: u.id, name: u.name }));

          callback({
            roomId,
            users: roomUsers,
            // Send router RTP capabilities to the client
            routerRtpCapabilities: room.router.rtpCapabilities,
          });

          console.log(`User ${name} (${userId}) joined room ${roomId}`);
        } catch (error) {
          console.error("Error joining room:", error);
          callback({ error: "Could not join room" });
        }
      }
    );

    // Handle WebRTC transport creation
    socket.on(
      "createTransport",
      async (payload: ISocketPayloads["createTransport"], callback) => {
        try {
          const { direction, userId, roomId } = payload;
          const room = this.roomService.getRoom(roomId);

          if (!room) {
            return callback({ error: "Room not found" });
          }

          const user = room.getUser(userId);
          if (!user) {
            return callback({ error: "User not found" });
          }

          // Create WebRTC transport
          const transport = await this.mediasoupService.createWebRtcTransport(
            room.router
          );

          // Store transport with its direction
          transport.appData = { ...transport.appData, direction };

          // Store transport
          user.transports.set(transport.id, transport);

          // Listen for transport events
          transport.on("dtlsstatechange", (dtlsState) => {
            if (dtlsState === "closed") {
              transport.close();
              user.transports.delete(transport.id);
            }
          });

          transport.on("@close", () => {
            console.log(`Transport ${transport.id} closed`);
            user.transports.delete(transport.id);
          });

          // Return transport parameters
          callback({
            transportId: transport.id,
            iceParameters: transport.iceParameters,
            iceCandidates: transport.iceCandidates,
            dtlsParameters: transport.dtlsParameters,
          });

          console.log(`Created ${direction} transport for user ${userId}`);
        } catch (error) {
          console.error("Error creating transport:", error);
          callback({ error: "Could not create transport" });
        }
      }
    );

    // Handle transport connection
    socket.on(
      "connectTransport",
      async (payload: ISocketPayloads["connectTransport"], callback) => {
        try {
          const { transportId, dtlsParameters, userId } = payload;

          // Find user's room
          const room = this.roomService.getUserRoom(userId);
          if (!room) {
            return callback({ error: "Room not found" });
          }

          const user = room.getUser(userId);
          if (!user) {
            return callback({ error: "User not found" });
          }

          const transport = user.transports.get(transportId);
          if (!transport) {
            return callback({ error: "Transport not found" });
          }

          // Connect transport
          await transport.connect({ dtlsParameters });
          callback({ connected: true });

          console.log(`Transport ${transportId} connected for user ${userId}`);
        } catch (error) {
          console.error("Error connecting transport:", error);
          callback({ error: "Could not connect transport" });
        }
      }
    );

    // Handle producer creation (sending audio/video)
    socket.on(
      "produceTrack",
      async (payload: ISocketPayloads["produceTrack"], callback) => {
        try {
          const { transportId, kind, rtpParameters, appData, userId } = payload;

          // Find user's room
          const room = this.roomService.getUserRoom(userId);
          if (!room) {
            return callback({ error: "Room not found" });
          }

          const user = room.getUser(userId);
          if (!user) {
            return callback({ error: "User not found" });
          }

          const transport = user.transports.get(transportId);
          if (!transport) {
            return callback({ error: "Transport not found" });
          }

          // Create producer
          const producer = await transport.produce({
            kind: kind as MediaKind,
            rtpParameters,
            appData: {
              ...appData,
              userId,
            },
          });

          // Store producer
          user.producers.set(producer.id, producer);

          // Listen for producer events
          producer.on("transportclose", () => {
            producer.close();
            user.producers.delete(producer.id);
          });

          // Notify other users in room about new producer
          socket.to(room.id).emit("newProducer", {
            producerId: producer.id,
            userId,
            kind,
            appData,
          });

          callback({ producerId: producer.id });

          console.log(`User ${userId} produced ${kind} track ${producer.id}`);
        } catch (error) {
          console.error("Error producing track:", error);
          callback({ error: "Could not produce track" });
        }
      }
    );

    // Handle consumer creation (receiving audio/video)
    socket.on(
      "consumeTrack",
      async (payload: ISocketPayloads["consumeTrack"], callback) => {
        try {
          const { producerId, rtpCapabilities, userId, roomId } = payload;

          const room = this.roomService.getRoom(roomId);
          if (!room) {
            return callback({ error: "Room not found" });
          }

          const user = room.getUser(userId);
          if (!user) {
            return callback({ error: "User not found" });
          }

          // Check if router can consume the producer
          if (!room.router.canConsume({ producerId, rtpCapabilities })) {
            return callback({ error: "Cannot consume producer" });
          }

          // Find a receive transport
          let receiveTransport;
          for (const [, transport] of user.transports) {
            // We'll use the first transport for simplicity
            // In a real app, you might want to track send vs receive transports
            if (transport.appData.direction === "receive") {
              receiveTransport = transport;
              break;
            }
          }

          if (!receiveTransport) {
            return callback({ error: "No receive transport found" });
          }

          // Create consumer
          const consumer = await receiveTransport.consume({
            producerId,
            rtpCapabilities,
            paused: true, // Start paused to avoid initial packet loss
          });

          // Store consumer
          user.consumers.set(consumer.id, consumer);

          // Listen for consumer events
          consumer.on("transportclose", () => {
            consumer.close();
            user.consumers.delete(consumer.id);
          });

          // Listen for producer close
          consumer.on("producerclose", () => {
            consumer.close();
            user.consumers.delete(consumer.id);
            socket.emit("consumerClosed", { consumerId: consumer.id });
          });

          // Return consumer parameters
          callback({
            consumerId: consumer.id,
            producerId,
            kind: consumer.kind,
            rtpParameters: consumer.rtpParameters,
            type: consumer.type,
            producerPaused: consumer.producerPaused,
          });

          // Resume the consumer after a short delay
          await consumer.resume();

          console.log(
            `User ${userId} consuming track from producer ${producerId}`
          );
        } catch (error) {
          console.error("Error consuming track:", error);
          callback({ error: "Could not consume track" });
        }
      }
    );

    // Handle producer pause
    socket.on(
      "pauseProducer",
      async (payload: ISocketPayloads["pauseProducer"], callback) => {
        try {
          const { producerId, userId } = payload;

          // Find user's room
          const room = this.roomService.getUserRoom(userId);
          if (!room) {
            return callback({ error: "Room not found" });
          }

          const user = room.getUser(userId);
          if (!user) {
            return callback({ error: "User not found" });
          }

          const producer = user.producers.get(producerId);
          if (!producer) {
            return callback({ error: "Producer not found" });
          }

          await producer.pause();

          // Notify other users in room
          socket.to(room.id).emit("producerPaused", {
            producerId,
            userId,
          });

          callback({ paused: true });
          console.log(`User ${userId} paused producer ${producerId}`);
        } catch (error) {
          console.error("Error pausing producer:", error);
          callback({ error: "Could not pause producer" });
        }
      }
    );

    // Handle producer resume
    socket.on(
      "resumeProducer",
      async (payload: ISocketPayloads["resumeProducer"], callback) => {
        try {
          const { producerId, userId } = payload;

          // Find user's room
          const room = this.roomService.getUserRoom(userId);
          if (!room) {
            return callback({ error: "Room not found" });
          }

          const user = room.getUser(userId);
          if (!user) {
            return callback({ error: "User not found" });
          }

          const producer = user.producers.get(producerId);
          if (!producer) {
            return callback({ error: "Producer not found" });
          }

          await producer.resume();

          // Notify other users in room
          socket.to(room.id).emit("producerResumed", {
            producerId,
            userId,
          });

          callback({ resumed: true });
          console.log(`User ${userId} resumed producer ${producerId}`);
        } catch (error) {
          console.error("Error resuming producer:", error);
          callback({ error: "Could not resume producer" });
        }
      }
    );

    // Handle producer close (stopping video/audio)
    socket.on(
      "closeProducer",
      async (payload: ISocketPayloads["closeProducer"], callback) => {
        try {
          const { producerId, userId } = payload;

          // Find user's room
          const room = this.roomService.getUserRoom(userId);
          if (!room) {
            return callback({ error: "Room not found" });
          }

          const user = room.getUser(userId);
          if (!user) {
            return callback({ error: "User not found" });
          }

          const producer = user.producers.get(producerId);
          if (!producer) {
            return callback({ error: "Producer not found" });
          }

          producer.close();
          user.producers.delete(producerId);

          callback({ closed: true });
          console.log(`User ${userId} closed producer ${producerId}`);
        } catch (error) {
          console.error("Error closing producer:", error);
          callback({ error: "Could not close producer" });
        }
      }
    );

    // Handle client reconnection attempts
    socket.on(
      "getRouterRtpCapabilities",
      async (payload: { roomId: string }, callback) => {
        try {
          const { roomId } = payload;
          const room = this.roomService.getRoom(roomId);

          if (!room) {
            return callback({ error: "Room not found" });
          }

          callback({ rtpCapabilities: room.router.rtpCapabilities });
        } catch (error) {
          console.error("Error getting router capabilities:", error);
          callback({ error: "Could not get router capabilities" });
        }
      }
    );

    // Handle client disconnect
    socket.on("disconnect", () => {
      console.log(`Client disconnected: ${socket.id}`);

      // Find all rooms this socket is in
      for (const room of this.roomService.getRooms()) {
        // Find user with this socket
        let userId: string | undefined;

        for (const [id, user] of room.users.entries()) {
          if (user.socket.id === socket.id) {
            userId = id;
            break;
          }
        }

        if (userId) {
          // Remove user from room
          room.removeUser(userId);

          // Notify other users in room
          socket.to(room.id).emit("userLeft", { userId });

          console.log(`User ${userId} left room ${room.id}`);

          // Delete room if empty
          if (room.isEmpty()) {
            this.roomService.deleteRoom(room.id);
            console.log(`Room ${room.id} deleted (empty)`);
          }
        }
      }
    });
  }
}
