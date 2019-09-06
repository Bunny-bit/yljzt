import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button} from 'antd';
import styles from './Activation.css';
const FormItem = Form.Item;
const create = Form.create;
function Confirmsucess({dispatch, form, en}) {
  const {getFieldDecorator} = form;
  let val = false;

  function getValByUrl(key) {

    let Reg = new RegExp(`${key}=(\\w*)`);
    // let Reg = /code=(\w*)/;  //正则
    let info = location.href.match(Reg);
    if (info && info.length == 2) {
      val = info[1];
    }
    // console.log(`${key},${val}`);
  }

  getValByUrl('key');
  dispatch({
    type: 'confirmsucess/setlogin',
    payload: {
      secretKey: val
    }
  });
  return (
    <div>
      {
        en ? <div>
          <Form>
            <div style={{marginTop: 20, height: '80px', lineHeight: '80px'}}><span
              style={{fontSize: '50px', color: 'red', float: 'left', fontWeight: '900'}}><Icon
              type="check-circle-o"/></span><span
              style={{fontSize: '30px', color: '#000', marginLeft: 20, float: 'left', fontWeight: '900'}}>激活成功</span>
            </div>
            <Link to="/"><Button type="primary" className={styles.login}>
              返回登录
            </Button></Link>
          </Form>
        </div>
          :
          <div>
            <Form>
              <div style={{marginTop: 20, height: '80px', lineHeight: '80px'}}><span
                style={{fontSize: '50px', color: 'red', float: 'left', fontWeight: '900'}}><Icon type="check-circle-o"/></span><span
                style={{fontSize: '30px', color: '#000', marginLeft: 20, float: 'left', fontWeight: '900'}}>激活成功</span>
              </div>
              <Link to="/"><Button type="primary" className={styles.login}>
                返回登录
              </Button></Link>
            </Form>
          </div>
      }
    </div>
  )

}
Confirmsucess = connect((state) => {
  return {
    ...state.confirmsucess,
  }
})(Confirmsucess);

export default create()(Confirmsucess);
