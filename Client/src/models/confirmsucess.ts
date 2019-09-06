import {remoteUrl} from '../utils/url';
import {notification} from 'antd';
import {routerRedux} from 'dva/router';
import {Form, Icon, Input, Button} from 'antd';
import * as api from './../api/api';
import {createApiAuthParam} from './../api/apiUtil.js';
export default {
  namespace: 'confirmsucess',
  state: {
    en: false,
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
    //  老的邮件激活逻辑   废弃
    //*setlogin({payload}, {call, put}) {
    //  const data  = yield call(...createApiAuthParam({
    //    method: new api.AccountApi().appAccountConfirmEmail,
    //    payload: payload
    //  }));
    //  if (data.success) {
    //    yield put({
    //      type: 'setState',
    //      payload: {
    //        en: true
    //      }
    //    });
    //  }
    //},

  },
  subscriptions: {
    setup({dispatch, history}) {
      window.dispatch = dispatch;
      return history.listen(({pathname, state}) => {
        if (pathname.toLowerCase() == '/confirm'.toLowerCase()) {
          dispatch({
            type: 'setlogin'
          });
        }
      });
    },
  },
};
