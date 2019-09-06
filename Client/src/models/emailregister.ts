import {routerRedux} from 'dva/router';
import {notification} from 'antd';
import * as api from './../api/api';
import {createApiAuthParam} from './../api/apiUtil.js';
export default {
  namespace: 'emailregister',
  state: {},
  subscriptions: {
    setup({dispatch, history}) {

    },
  },

  effects: {
    *login({payload}, {call, put}) {
      const data = yield call(...createApiAuthParam({
        method: new api.RegisterApi().appRegisterRegisterByEmail,
        payload: payload
      }));
      if (data.success) {
        notification.success({
          message: '注册成功',
          description: '恭喜你注册成功,请前往激活',
        });
        yield put(routerRedux.push("/sucess"))
      }
    },
  },

  reducers: {
    setState(state, {payload}) {
      return {
        ...state,
        ...payload
      }
    },
  },
};
