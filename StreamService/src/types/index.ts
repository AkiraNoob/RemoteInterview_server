import {
  Consumer,
  MediaKind,
  Producer,
  Router,
  RtpCodecCapability,
  Transport,
  WebRtcTransport,
  Worker,
} from "mediasoup/node/lib/types";
import { Socket } from "socket.io";

export { MediaKind } from "mediasoup/node/lib/types";

export interface IMediasoupService {
  start(): Promise<void>;
  getWorker(): Worker;
  createRouter(): Promise<Router>;
  createWebRtcTransport(
    router: Router,
    options?: any
  ): Promise<WebRtcTransport>;
}

export interface IUser {
  id: string;
  name: string;
  socket: Socket;
  transports: Map<string, Transport>;
  producers: Map<string, Producer>;
  consumers: Map<string, Consumer>;
  rtpCapabilities?: any;
}

export interface IProducerOptions {
  kind: MediaKind;
  rtpParameters: any;
  appData?: any;
}

export interface ITransportOptions {
  id: string;
  iceParameters: any;
  iceCandidates: any;
  dtlsParameters: any;
}

export interface IRoom {
  id: string;
  name: string;
  users: Map<string, IUser>;
  router: Router;
  addUser(user: IUser): void;
  removeUser(userId: string): void;
  getUser(userId: string): IUser | undefined;
  getUsers(): IUser[];
  isEmpty(): boolean;
}

export interface IRoomService {
  createRoom(name: string): Promise<IRoom>;
  getRoom(roomId: string): IRoom | undefined;
  deleteRoom(roomId: string): boolean;
  getRooms(): IRoom[];
  getUserRoom(userId: string): IRoom | undefined;
}

export interface IJoinRoomPayload {
  roomId: string;
  userId: string;
  name: string;
}

export interface ISocketPayloads {
  joinRoom: IJoinRoomPayload;
  createTransport: {
    direction: "send" | "receive";
    userId: string;
    roomId: string;
  };
  connectTransport: {
    transportId: string;
    dtlsParameters: any;
    userId: string;
  };
  produceTrack: {
    transportId: string;
    kind: MediaKind;
    rtpParameters: any;
    appData: any;
    userId: string;
  };
  consumeTrack: {
    producerId: string;
    rtpCapabilities: any;
    userId: string;
    roomId: string;
  };
  pauseProducer: {
    producerId: string;
    userId: string;
  };
  resumeProducer: {
    producerId: string;
    userId: string;
  };
  closeProducer: {
    producerId: string;
    userId: string;
  };
}

export interface IMediaCodecCapability extends RtpCodecCapability {
  kind: "audio" | "video";
}
