import { Router } from "mediasoup/node/lib/types";
import { v4 as uuidv4 } from "uuid";
import { IRoom, IRoomService, IUser } from "../types";
import { MediasoupService } from "./mediasoup-service";

class Room implements IRoom {
  id: string;
  name: string;
  users: Map<string, IUser>;
  router: Router;

  constructor(id: string, name: string, router: Router) {
    this.id = id;
    this.name = name;
    this.users = new Map<string, IUser>();
    this.router = router;
  }

  addUser(user: IUser): void {
    this.users.set(user.id, user);
  }

  removeUser(userId: string): void {
    const user = this.users.get(userId);
    if (user) {
      // Close all user's transports, producers, and consumers
      user.transports.forEach((transport) => {
        transport.close();
      });
      this.users.delete(userId);
    }
  }

  getUser(userId: string): IUser | undefined {
    return this.users.get(userId);
  }

  getUsers(): IUser[] {
    return Array.from(this.users.values());
  }

  isEmpty(): boolean {
    return this.users.size === 0;
  }
}

export class RoomService implements IRoomService {
  private rooms: Map<string, IRoom>;
  private mediasoupService: MediasoupService;

  constructor(mediasoupService: MediasoupService) {
    this.rooms = new Map<string, IRoom>();
    this.mediasoupService = mediasoupService;
  }

  async createRoom(name: string): Promise<IRoom> {
    const roomId = uuidv4();
    const router = await this.mediasoupService.createRouter();
    const room = new Room(roomId, name, router);
    this.rooms.set(roomId, room);

    console.log(`Room created: ${name} (${roomId})`);
    return room;
  }

  getRoom(roomId: string): IRoom | undefined {
    return this.rooms.get(roomId);
  }

  deleteRoom(roomId: string): boolean {
    return this.rooms.delete(roomId);
  }

  getRooms(): IRoom[] {
    return Array.from(this.rooms.values());
  }

  getUserRoom(userId: string): IRoom | undefined {
    for (const room of this.rooms.values()) {
      if (room.getUser(userId)) {
        return room;
      }
    }
    return undefined;
  }
}
