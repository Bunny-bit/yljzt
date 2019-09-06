/**
 * app.js
 * Created by 李廷旭 on 2017/8/7 16:46
 * 描述: 这里是通用的model
 */
export default {
  namespace: 'app',
  state: {
    locationState: {},  //上级页面传给下级页面的state
  },
  reducers: {
    save(state, locationState) {
      return {
        ...state,
        ...locationState
      };
    },
  },
  effects: {},
  subscriptions: {
    setup({dispatch, history, router}) {
      return history.listen(({pathname, state}) => {
        console.log('=================华丽的分割线====================');
        // console.log('state', state);
        // console.log('state', JSON.stringify(state));
        //上级跳转的数据传给下个页面
        if (state) {
          dispatch({
            type: 'save',
            locationState: state
          });
        }
      });
    }
  },
};
