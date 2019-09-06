import { routerRedux } from 'dva/router';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';
import { homePageUrl } from '../utils/url';
export default {
  namespace: 'sendemail',
  state: {},
  reducers: {
    setState(state, { payload }) {
      return {
        ...state,
        ...payload
      };
    }
  },
  effects: {
    //  老的激活逻辑，废弃
    //*sentpass({ payload }, { call, put }) {
    //	const { success, result } = yield call(
    //		...createApiAuthParam({
    //			method: new api.AccountApi().appAccountSendConfirmEmail,
    //			payload: payload
    //		})
    //	);
    //	if (success) {
    //	}
    //},
    //短信
    *sendVerificationCode({ payload }, { call, put }) {
      const { success, result } = yield call(
        ...createApiAuthParam({
          method: new api.ActiveApi().appActiveSendConfirmPhoneNumberByCode,
          payload: payload
        })
      );
      if (success) {
        if (payload.sufn) {
          payload.sufn();
        }
      } else {
        if (payload.fafn) {
          payload.fafn();
        }
      }
    },
    *confirmPhoneNumberByCode({ payload }, { call, put }) {
      const { success, result } = yield call(
        ...createApiAuthParam({
          method: new api.ActiveApi().appActiveConfirmPhoneNumberByCode,
          payload: payload,
        })
      );
      if (success) {
        if (result && result.isLogin) {
          yield put(routerRedux.push(homePageUrl));
        } else if (payload.sufn) {
          payload.sufn();
        }
      } else {
        if (payload.fafn) {
          payload.fafn();
        }
      }
    },

    //邮箱
    *sendConfirmEmailCode({ payload }, { call, put }) {
      const { success, result } = yield call(
        ...createApiAuthParam({
          method: new api.ActiveApi().appActiveSendConfirmEmailCode,
          payload: payload
        })
      );
      if (success) {
        if (payload.sufn) {
          payload.sufn();
        }
      } else {
        if (payload.fafn) {
          payload.fafn();
        }
      }
    },
    *confirmEmailByCode({ payload }, { call, put }) {
      const { success, result } = yield call(

        ...createApiAuthParam({
          method: new api.ActiveApi().appActiveConfirmEmailByCode,
          payload: payload
        })
      );
      if (success) {
        if (result&&result.isLogin) {
          yield put(routerRedux.push(homePageUrl));
        } else if (payload.sufn) {
          payload.sufn();
        }
      } else {
        if (payload.fafn) {
          payload.fafn();
        }
      }
    }
  },
  subscriptions: {
    setup({ dispatch, history }) {
      return history.listen(({ pathname, state }) => { });
    }
  }
};
