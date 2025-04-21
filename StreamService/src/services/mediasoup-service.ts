import * as mediasoup from "mediasoup";
import { Router, WebRtcTransport, Worker } from "mediasoup/node/lib/types";
import { config } from "../config/mediasoup";
import { IMediaCodecCapability, IMediasoupService } from "../types";

export class MediasoupService implements IMediasoupService {
  private workers: Worker[] = [];
  private nextWorkerIndex = 0;

  public async start(): Promise<void> {
    // Launch mediasoup workers
    for (let i = 0; i < config.mediasoup.numWorkers; i++) {
      const worker = await mediasoup.createWorker({
        logLevel: config.mediasoup.worker.logLevel as any,
        logTags: config.mediasoup.worker.logTags as any,
        rtcMinPort: config.mediasoup.worker.rtcMinPort,
        rtcMaxPort: config.mediasoup.worker.rtcMaxPort,
      });

      worker.on("died", () => {
        console.error(
          `mediasoup worker ${worker.pid} died, exiting in 2 seconds...`
        );
        setTimeout(() => process.exit(1), 2000);
      });

      this.workers.push(worker);
      console.log(`mediasoup worker ${worker.pid} started`);
    }
  }

  public getWorker(): Worker {
    // Round-robin selection of mediasoup worker
    const worker = this.workers[this.nextWorkerIndex];

    // Update next worker index for round-robin
    if (++this.nextWorkerIndex === this.workers.length) {
      this.nextWorkerIndex = 0;
    }

    return worker;
  }

  public async createRouter(): Promise<Router> {
    const worker = this.getWorker();
    return await worker.createRouter({
      mediaCodecs: config.mediasoup.router
        .mediaCodecs as IMediaCodecCapability[],
    });
  }

  public async createWebRtcTransport(
    router: Router,
    options?: any
  ): Promise<WebRtcTransport> {
    const transportOptions = {
      ...config.mediasoup.webRtcTransport,
      ...options,
      enableUdp: true,
      enableTcp: true,
      preferUdp: true,
    };

    return await router.createWebRtcTransport(transportOptions);
  }
}
