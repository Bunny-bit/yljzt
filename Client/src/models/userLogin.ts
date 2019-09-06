import moment from 'moment';
import * as api from './../api/api';
import {createApiAuthParam} from './../api/apiUtil.js';

export default {
  namespace: 'userLogin',
  state: {
    visible: false,
    items: [],
    record: {},
    //分页信息
    pagination: {
      total: 0,
      current: 1,
      pageSize: 10,
      showSizeChanger: true,
      showQuickJumper: true,
      showTotal: total => `共 ${total} 条`,
      size: 'large'
    }
  },
  reducers: {
    setState(state, {payload}) {
      return {
        ...state,
        ...payload
      }
    },
    setPages(state, {payload}) {
      return {
        ...state,
        items: payload.items,
        pagination: {
          ...state.pagination,
          ...payload.pagination,
        },
        visible: true,
      }
    }
  },
  effects: {
    *getUserLogins({payload}, {call, put}) {
      yield put({
        type: 'setState',
        payload: {visible: true,}
      })
      var body = {
        sorting: payload.field ? payload.field + " " + payload.order.replace("end", "") : "",
        maxResultCount: payload.pageSize,
        skipCount: payload.pageSize * (payload.current - 1),
      }
      if (payload && payload.dateRange) {
        body.startDate = payload.dateRange[0].format("YYYY-MM-DD");
        body.endDate = payload.dateRange[1].format("YYYY-MM-DD 23:59:59");
      }
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.UserLoginApi().appUserLoginGetRecentUserLoginAttempts,
        payload:body
      }));
      if (success) {
        yield put({
          type: 'setPages',
          payload: {
            items: result.items,
            pagination: {
              ...payload,
              total: result.totalCount,
              current: payload.current,
              pageSize: payload.pageSize,
            },
          }
        });
      }
    },
  },
  subscriptions: {},
};
