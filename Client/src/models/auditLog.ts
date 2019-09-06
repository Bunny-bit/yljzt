import moment from 'moment';
import * as api from './../api/api';
import {createApiAuthParam} from './../api/apiUtil.js';

export default {
  namespace: 'auditLog',
  state: {
    expand: false,
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
    },
    search: {},
    filters: {},
    sorter: {},
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
        }
      }
    }
  },
  effects: {
    *getAuditLogs({payload}, {call, put}) {
      var body = {
        userName: payload.userName,
        serviceName: payload.serviceName,
        methodName: payload.methodName,
        browserInfo: payload.browserInfo,
        hasException: payload.hasException,
        minExecutionDuration: payload.minExecutionDuration,
        maxExecutionDuration: payload.maxExecutionDuration,
        sorting: payload.field ? payload.field + " " + payload.order.replace("end", "") : "",
        maxResultCount: payload.pageSize,
        skipCount: payload.pageSize * (payload.current - 1),
      }
      if (payload && !payload.isAdvancedSearch && payload.dateRange) {
        body.startDate = payload.dateRange[0].format("YYYY-MM-DD");
        body.endDate = payload.dateRange[1].format("YYYY-MM-DD 23:59:59");
      }
      if (payload && payload.isAdvancedSearch && payload.dateRange1) {
        body.startDate = payload.dateRange1[0].format("YYYY-MM-DD");
        body.endDate = payload.dateRange1[1].format("YYYY-MM-DD 23:59:59");
      }
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.AuditLogApi().appAuditLogGetAuditLogs,
        payload: body
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
            }
          }
        });
      }
    },
    *getAuditLogsToExcel({payload}, {call, put}) {
      var body = {
        userName: payload.userName,
        serviceName: payload.serviceName,
        methodName: payload.methodName,
        browserInfo: payload.browserInfo,
        hasException: payload.hasException,
        minExecutionDuration: payload.minExecutionDuration,
        maxExecutionDuration: payload.maxExecutionDuration,
        sorting: payload.field ? payload.field + " " + payload.order.replace("end", "") : "",
        maxResultCount: payload.pageSize,
        skipCount: payload.pageSize * (payload.current - 1),
      }
      if (payload && !payload.isAdvancedSearch && payload.dateRange) {
        body.startDate = payload.dateRange[0].format("YYYY-MM-DD");
        body.endDate = payload.dateRange[1].format("YYYY-MM-DD 23:59:59");
      }
      if (payload && payload.isAdvancedSearch && payload.dateRange1) {
        body.startDate = payload.dateRange1[0].format("YYYY-MM-DD");
        body.endDate = payload.dateRange1[1].format("YYYY-MM-DD 23:59:59");
      }
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.AuditLogApi().appAuditLogGetAuditLogsToExcel,
        payload: body
      }));
      if (success) {

        yield put({
          type: 'download/downloadTempFile',
          payload: {
            ...result
          }
        });
      }
    },
  },
  subscriptions: {
    setup({dispatch, history}) {
      return history.listen(({pathname, state}) => {
        if (pathname.toLowerCase() == '/auditLog'.toLowerCase()) {
          dispatch({
            type: 'getAuditLogs',
            payload: {
              current: 1,
              pageSize: 10
            }
          });
        }
      });
    },
  },
};
