import { routerRedux } from 'dva/router';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';
import { homePageUrl } from '../utils/url';
export default {
  namespace: 'yljzt',
  state: {
    visible:false,
    currentTimuId:0
  },
  reducers: {
    setState(state, { payload }) {
      return {
        ...state,
        ...payload
      };
    }
  },
  effects: {
    
    *createXuanxiang({ payload }, { call, put }) {
      const { success, result } = yield call(
        ...createApiAuthParam({
          method: new api.XuanxiangApi().appXuanxiangCreate,
          payload: payload,
        })
      );
      if (success) {
        yield put({
          type:"setState",
          payload:{visible:false}
        })
        window.location.reload();
      } 
    },
    *deleteXuanxiang({ payload }, { call, put }) {
      const { success, result } = yield call(
        ...createApiAuthParam({
          method: new api.XuanxiangApi().appXuanxiangDelete,
          payload: payload,
        })
      );
      if (success) {
        window.location.reload();
      } 
    },
  },
  subscriptions: {
    setup({ dispatch, history }) {
    }
  }
};
