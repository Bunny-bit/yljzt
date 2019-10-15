import moment from 'moment';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';

export default {
    namespace: 'datitu',
    state: {
        zhengQueShuZuiGao: [],
        canyuzhanbi: [],
        timuDaduiRenshu: [],
        canyu: [],
        zhengque:[],
        forceFit: true,
        width: 500,
        height: 450,
        plotCfg: {
            margin: [20, 60, 80, 120]
        },
    },
    reducers: {
        setState(state, { payload }) {
            return {
                ...state,
                ...payload
            }
        }
    },
    effects: {
        *getZhengQueShuZuiGao({ payload }, { call, put }) {
            const { success, result } = yield call(...createApiAuthParam({
                method: new api.YljztApi().appYljztGetZhengQueShuZuiGao,
                payload: { payload }
            }));
            if (success) {
                yield put({
                    type: 'setState',
                    payload: {
                        zhengQueShuZuiGao: result,
                    }
                });
            }
        },
        *getTimuRenshu({ payload }, { call, put }) {
            const { success, result } = yield call(...createApiAuthParam({
                method: new api.YljztApi().appYljztGetTimuRenshu,
                payload: { payload }
            }));
            if (success) {
                yield put({
                    type: 'setState',
                    payload: {
                        timuDaduiRenshu: result,
                    }
                });
            }
        },
        *getXueyuanCanyu({ payload }, { call, put }) {
            const { success, result } = yield call(...createApiAuthParam({
                method: new api.YljztApi().appYljztGetXueyuanCanyu,
                payload: { payload }
            }));
            if (success) {
                yield put({
                    type: 'setState',
                    payload: {
                        canyu: result,
                    }
                });
            }
        },
        *getXueyuanCanyuZhanbi({ payload }, { call, put }) {
            const { success, result } = yield call(...createApiAuthParam({
                method: new api.YljztApi().appYljztGetXueyuanCanyu,
                payload: { payload }
            }));
            var total = 0;
            for (var i in result) {
                total += result[i].renshu;
            }
            for (var i in result) {
                if (total == 0) {
                    result[i].zhanbi = 0
                } else {
                    result[i].zhanbi = result[i].renshu / total;
                }
            }
            if (success) {
                yield put({
                    type: 'setState',
                    payload: {
                        canyuzhanbi: result,
                    }
                });
            }
        },
        *getXueyuanZhengquelv({ payload }, { call, put }) {
            const { success, result } = yield call(...createApiAuthParam({
                method: new api.YljztApi().appYljztGetXueyuanZhengquelv,
                payload: { payload }
            }));
            if (success) {
                yield put({
                    type: 'setState',
                    payload: {
                        zhengque: result,
                    }
                });
            }
        }
    },
    subscriptions: {
        setup({ dispatch, history }) {
            return history.listen(({ pathname, state }) => {
                if (pathname.toLowerCase() == '/datitu'.toLowerCase()) {
                    dispatch({
                        type: 'getZhengQueShuZuiGao'
                    });
                    dispatch({
                        type: 'getTimuRenshu'
                    });
                    dispatch({
                        type: 'getXueyuanCanyu'
                    });
                    dispatch({
                        type: 'getXueyuanCanyuZhanbi'
                    });
                    dispatch({
                        type: 'getXueyuanZhengquelv'
                    });
                }
            });
        }
    },
};
