import { createWorker } from "mediasoup";
import { MEDIA_CODECS } from "../config/mediasoup";
import { MediaSoupWorkerWithRouter, Room, Rooms, Workers } from "../types";

export async function createMediaSoupWorker(): Promise<MediaSoupWorkerWithRouter> {
  const worker = await createWorker({
    logLevel: "warn",
    logTags: ["info", "ice", "dtls", "rtp", "srtp", "rtcp", "sctp"],
    rtcMinPort: 40000,
    rtcMaxPort: 49999,
  });

  worker.on("died", () => {
    console.error("Mediasoup worker died, exiting...");
    process.exit(1);
  });

  return worker;
}

export async function getOrCreateRoom(
  roomId: string,
  workers: Workers,
  rooms: Rooms
): Promise<Room> {
  let room = rooms.get(roomId);

  if (!room) {
    if (workers.length === 0) {
      const worker = await createMediaSoupWorker();
      workers.push(worker);
    }
    const worker = workers[Math.floor(Math.random() * workers.length)];
    const router = await worker.createRouter({ mediaCodecs: MEDIA_CODECS });
    room = { id: roomId, router, users: new Map() };
    rooms.set(roomId, room);
    console.log(`Created room: ${roomId}`);
  }

  return room;
}
