import { Request, Response } from "express";
import { RoomService } from "../services/room-service";

export class RoomController {
  private roomService: RoomService;

  constructor(roomService: RoomService) {
    this.roomService = roomService;
  }

  public getRooms = async (req: Request, res: Response): Promise<void> => {
    try {
      const rooms = this.roomService.getRooms().map((room) => ({
        id: room.id,
        name: room.name,
        userCount: room.getUsers().length,
      }));

      res.status(200).json({ rooms });
    } catch (error) {
      console.error("Error getting rooms:", error);
      res.status(500).json({ error: "Could not get rooms" });
    }
  };

  public createRoom = async (req: Request, res: Response): Promise<void> => {
    try {
      const { name } = req.body;

      if (!name) {
        res.status(400).json({ error: "Room name is required" });
        return;
      }

      const room = await this.roomService.createRoom(name);

      res.status(201).json({
        id: room.id,
        name: room.name,
      });
    } catch (error) {
      console.error("Error creating room:", error);
      res.status(500).json({ error: "Could not create room" });
    }
  };

  public getRoom = async (req: Request, res: Response): Promise<void> => {
    try {
      const { id } = req.params;
      const room = this.roomService.getRoom(id);

      if (!room) {
        res.status(404).json({ error: "Room not found" });
        return;
      }

      res.status(200).json({
        id: room.id,
        name: room.name,
        users: room.getUsers().map((user) => ({
          id: user.id,
          name: user.name,
        })),
      });
    } catch (error) {
      console.error("Error getting room:", error);
      res.status(500).json({ error: "Could not get room" });
    }
  };
}
