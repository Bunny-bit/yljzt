import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button} from 'antd';
import styles from './Activation.css';
const FormItem = Form.Item;
const create = Form.create;
function Actsucess({dispatch, form}) {
  const {getFieldDecorator} = form;

  function handleSubmit() {
    console.log('1')
  }

  return (
    <div>
      <Form onSubmit={handleSubmit} className={`${styles.formbox} login-form`}>
        <div style={{marginTop: 20, height: '80px', lineHeight: '80px'}}><span
          style={{fontSize: '50px', color: 'red', float: 'left', fontWeight: '900'}}><Icon
          type="check-circle-o"/></span><span
          style={{fontSize: '30px', color: '#000', marginLeft: 20, float: 'left', fontWeight: '900'}}>发送成功</span></div>
        <div style={{fontSize: '22px', color: '#000'}}>激活邮件已发送到您的邮箱</div>
        <div style={{fontSize: '18px', color: '#8a92a3', marginTop: 20, marginBottom: 20}}>
          请在24小时内登录您的邮箱接收邮件，点击激活链接后即可激活账户。
        </div>
      </Form>
    </div>
  )
}
Actsucess = connect((state) => {
  return {
    ...state.Actsucess,
  }
})(Actsucess);

export default create()(Actsucess);
