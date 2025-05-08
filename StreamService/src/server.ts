import cors from "cors";
import express from "express";
import http from "http";
import { Socket, Server as SocketIOServer } from "socket.io";

// Define the structure for storing room information
interface RoomInfo {
  users: Set<string>; // Set of socket IDs for users in the room
}

// Define the structure for storing all rooms
interface Rooms {
  [roomId: string]: RoomInfo;
}

const app = express();
// Enable CORS for all origins (adjust in production)
app.use(cors());

const server = http.createServer(app);
const io = new SocketIOServer(server, {
  cors: {
    origin: "*", // Allow connections from any origin - Adjust for production!
    methods: ["GET", "POST"],
  },
});

// In-memory store for rooms and users. Replace with a database for persistence.
const rooms: Rooms = {};

// Basic route for testing the server
app.get("/", (req, res) => {
  res.send("WebRTC Signaling Server is running!");
});

io.on("connection", (socket: Socket) => {
  console.log(`User connected: ${socket.id}`);

  // --- Room Management ---

  socket.on("join-room", (roomId: string) => {
    console.log(`User ${socket.id} attempting to join room ${roomId}`);

    // Create room if it doesn't exist
    if (!rooms[roomId]) {
      rooms[roomId] = { users: new Set() };
      console.log(`Room ${roomId} created.`);
    }

    // Add user to the room
    rooms[roomId].users.add(socket.id);
    socket.join(roomId);
    console.log(
      `User ${socket.id} joined room ${roomId}. Users in room: ${Array.from(
        rooms[roomId].users
      )}`
    );

    // Get list of other users already in the room
    const otherUsers = Array.from(rooms[roomId].users).filter(
      (id) => id !== socket.id
    );

    // Notify the new user about existing users (so they can initiate connections)
    socket.emit("existing-users", otherUsers);
    console.log(`Sent existing users ${otherUsers} to ${socket.id}`);

    // Notify existing users about the new user
    socket.to(roomId).emit("user-joined", socket.id);
    console.log(`Notified room ${roomId} that ${socket.id} joined.`);
  });

  // --- WebRTC Signaling ---

  // Relay offer to a specific user
  socket.on(
    "offer",
    (payload: { targetUserId: string; callerUserId: string; signal: any }) => {
      console.log(
        `Relaying offer from ${payload.callerUserId} to ${payload.targetUserId}`
      );
      io.to(payload.targetUserId).emit("offer-received", {
        signal: payload.signal,
        callerUserId: payload.callerUserId,
      });
    }
  );

  // Relay answer back to the caller
  socket.on(
    "answer",
    (payload: { targetUserId: string; calleeUserId: string; signal: any }) => {
      console.log(
        `Relaying answer from ${payload.calleeUserId} to ${payload.targetUserId}`
      );
      io.to(payload.targetUserId).emit("answer-received", {
        signal: payload.signal,
        calleeUserId: payload.calleeUserId,
      });
    }
  );

  // Relay ICE candidates
  socket.on(
    "ice-candidate",
    (payload: {
      targetUserId: string;
      candidate: any;
      senderUserId: string;
    }) => {
      // console.log(`Relaying ICE candidate from ${payload.senderUserId} to ${payload.targetUserId}`); // Can be very verbose
      io.to(payload.targetUserId).emit("ice-candidate-received", {
        candidate: payload.candidate,
        senderUserId: payload.senderUserId,
      });
    }
  );

  // --- Disconnection ---

  socket.on("disconnecting", () => {
    console.log(`User disconnecting: ${socket.id}`);
    // Find which rooms the user was in
    socket.rooms.forEach((roomId) => {
      // Skip the default room which is the socket's own ID
      if (roomId === socket.id) return;

      if (rooms[roomId]) {
        // Remove user from the room's user list
        rooms[roomId].users.delete(socket.id);
        console.log(
          `User ${
            socket.id
          } removed from room ${roomId}. Users left: ${Array.from(
            rooms[roomId].users
          )}`
        );

        // Notify remaining users in the room
        socket.to(roomId).emit("user-left", socket.id);
        console.log(`Notified room ${roomId} that ${socket.id} left.`);

        // Optional: Clean up empty rooms
        if (rooms[roomId].users.size === 0) {
          delete rooms[roomId];
          console.log(`Room ${roomId} deleted as it is empty.`);
        }
      }
    });
  });

  socket.on("disconnect", () => {
    console.log(`User disconnected: ${socket.id}`);
    // Additional cleanup if needed (already handled in 'disconnecting')
  });
});

const PORT = process.env.PORT || 3001; // Use environment variable or default port

server.listen(PORT, () => {
  console.log(`🚀 Signaling server listening on port ${PORT}`);
});
