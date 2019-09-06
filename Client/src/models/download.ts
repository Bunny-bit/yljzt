import {remoteUrl} from '../utils/url';

export default {
  namespace: 'download',
  state: {},
  reducers: {},
  effects: {
    *downloadTempFile({payload}, {call, put}) {

      window.location = `${remoteUrl}/File/DownloadTempFile?filename=${payload.fileName}&fileType=${payload.fileType}&fileToken=${payload.fileToken}`;
    },
  },
  subscriptions: {},
};
