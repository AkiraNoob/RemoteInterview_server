import cors from "cors";
import express from "express";
import helmet from "helmet";
import http from "http";
import { config } from "./config/mediasoup";
import { errorHandler } from "./middlewares/error-handler";
import { MediasoupService } from "./services/mediasoup-service";
import { RoomService } from "./services/room-service";
import { SocketHandler } from "./socket/socket-handler";
import { SocketServer } from "./socket/socket-server";

async function startServer() {
  try {
    // Initialize Express app
    const app = express();

    // Set up middlewares
    app.use(cors());
    app.use(helmet());
    app.use(express.json());

    // Create HTTP server
    const httpServer = http.createServer(app);

    // Initialize services
    const mediasoupService = new MediasoupService();
    await mediasoupService.start();

    const roomService = new RoomService(mediasoupService);

    // Initialize socket handler and server
    const socketHandler = new SocketHandler(roomService, mediasoupService);
    const socketServer = new SocketServer(httpServer, socketHandler);
    socketServer.start();

    // Initialize controllers
    // const roomController = new RoomController(roomService);

    // Set up routes
    app.get("/health", (req, res) => {
      res.status(200).json({ status: "ok" });
    });

    // Public routes
    // app.get("/rooms", roomController.getRooms);
    // app.get("/rooms/:id", roomController.getRoom);

    // // Protected routes
    // app.post("/rooms", roomController.createRoom);

    // Error handling
    app.use(errorHandler);

    // Start server
    httpServer.listen(config.port, () => {
      console.log(`Server listening on port ${config.port}`);
    });
  } catch (error) {
    console.error("Failed to start server:", error);
    process.exit(1);
  }
}

startServer();
