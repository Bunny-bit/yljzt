import {remoteUrl} from '../utils/url';
import {notification} from 'antd';
import {routerRedux} from 'dva/router';
import * as api from './../api/api';
import {createApiAuthParam} from './../api/apiUtil.js';
export default {
  namespace: 'callback',
  state: {
    loading: false,
    abled: false,
    hedhtml: '获取验证码'
  },
  reducers: {
    setState(state, {payload}) {
      return {
        ...state,
        ...payload
      };
    },
  },
  effects: {
    *setlogin({payload}, {call, put}) {
      const data = yield call(...createApiAuthParam({
        method: new api.RestPasswordApi().appRestPasswordSendPhoneNumberCode,
        payload: payload
      }));
      if (data.success) {
        notification.success({
          message: '发送成功',
          description: '验证码发送成功',
        });
      }
    },
  
    *setlogin3({payload}, {call, put}) {
      const data = yield call(...createApiAuthParam({
        method: new api.RestPasswordApi().appRestPasswordResetPasswordByPhoneNumber,
        payload: payload
      }));
      if (data.success) {
        notification.success({
          message: '找回密码成功',
          description: '请返回登录页面，进行登录',
        });
        yield put(routerRedux.push("/backsucess"))
      }
    },
  },
  subscriptions: {
    setup({dispatch, history}) {
      return history.listen(({pathname, state}) => {

      });
    },
  },
};
