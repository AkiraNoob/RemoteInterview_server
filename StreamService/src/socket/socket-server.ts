import { Server as HttpServer } from "http";
import { Server as SocketIOServer } from "socket.io";
import { SocketHandler } from "./socket-handler";

export class SocketServer {
  private io: SocketIOServer;
  private socketHandler: SocketHandler;

  constructor(httpServer: HttpServer, socketHandler: SocketHandler) {
    this.io = new SocketIOServer(httpServer, {
      cors: {
        origin: "*",
        methods: ["GET", "POST"],
      },
    });

    this.socketHandler = socketHandler;
  }

  public start(): void {
    this.io.on("connection", (socket) => {
      this.socketHandler.handleConnection(socket);
    });

    console.log("Socket.IO server started");
  }
}
