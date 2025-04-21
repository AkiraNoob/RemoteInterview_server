import os from "os";
const DEFAULT_PORT = 3000;

export const config = {
  port: parseInt(process.env.PORT || DEFAULT_PORT.toString(), 10),
  logLevel: process.env.LOG_LEVEL || "info",
  // mediasoup settings
  mediasoup: {
    // Number of workers
    numWorkers: Object.keys(os.cpus()).length,
    // mediasoup Worker settings
    worker: {
      rtcMinPort: parseInt(process.env.MEDIASOUP_MIN_PORT || "10000"),
      rtcMaxPort: parseInt(process.env.MEDIASOUP_MAX_PORT || "10100"),
      logLevel: "warn",
      logTags: ["info", "ice", "dtls", "rtp", "srtp", "rtcp"],
    },
    // mediasoup Router settings
    router: {
      mediaCodecs: [
        {
          kind: "audio",
          mimeType: "audio/opus",
          clockRate: 48000,
          channels: 2,
        },
        {
          kind: "video",
          mimeType: "video/VP8",
          clockRate: 90000,
          parameters: {
            "x-google-start-bitrate": 1000,
          },
        },
        {
          kind: "video",
          mimeType: "video/VP9",
          clockRate: 90000,
          parameters: {
            "profile-id": 2,
            "x-google-start-bitrate": 1000,
          },
        },
        {
          kind: "video",
          mimeType: "video/h264",
          clockRate: 90000,
          parameters: {
            "packetization-mode": 1,
            "profile-level-id": "4d0032",
            "level-asymmetry-allowed": 1,
            "x-google-start-bitrate": 1000,
          },
        },
      ],
    },
    // mediasoup WebRtcTransport settings
    webRtcTransport: {
      listenIps: { ip: "0.0.0.0", announcedIp: null },
      initialAvailableOutgoingBitrate: 1000000,
      minimumAvailableOutgoingBitrate: 600000,
      maxSctpMessageSize: 262144,
      maxIncomingBitrate: 1500000,
    },
  },
};
