import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button, Steps} from 'antd';
import styles from '../../Activation/Activation.css';
const create = Form.create;
function Backsucess({dispatch, form, current}) {


  return (
    <div>
      <div>
        <Form className={`${styles.formbox} login-form`}>
          <div style={{marginTop: 20, height: '80px', lineHeight: '80px'}}><span
            style={{fontSize: '50px', color: 'red', float: 'left', fontWeight: '900'}}><Icon
            type="check-circle-o"/></span><span
            style={{fontSize: '30px', color: '#000', marginLeft: 20, float: 'left', fontWeight: '900'}}>成功</span></div>
          <div style={{fontSize: '18px', color: '#8a92a3', marginTop: 20}}>请返回登录页面登录。</div>
          <Link to="/"><Button type="primary" htmlType="submit" className={styles.login}>
            返回登录页面
          </Button></Link>
        </Form>
      </div>
    </div>
  )
}
Backsucess = connect((state) => {
  return {
    ...state.backsucess,
  }
})(Backsucess);

export default create()(Backsucess);
