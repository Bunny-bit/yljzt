import {routerRedux} from 'dva/router';
import * as api from './../api/api';
import {createApiAuthParam} from './../api/apiUtil.js';
export default {
  namespace: 'rentbyemail',
  state: {},
  reducers: {
    setState(state, {payload}) {
      return {
        ...state,
        ...payload
      };
    },
  },
  effects: {
    *sentpass({payload}, {call, put, select}) {
      const backknow = yield select(state => state.backknow);
      const { success, result } = yield call(...createApiAuthParam({
        method: new api.RestPasswordApi().appRestPasswordResetPasswordByEmail,
        payload: payload
      }));
      if (success) {
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
