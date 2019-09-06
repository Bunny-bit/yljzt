import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button} from 'antd';
import styles from '../../Activation/Activation.css';
const FormItem = Form.Item;
const create = Form.create;
function Sucesssigiter({dispatch, form}) {
  const {getFieldDecorator} = form;

  return (
    <div>
      <Form className={`${styles.formbox} login-form`}>
        <div style={{marginTop: 20, height: '80px', lineHeight: '80px'}}><span
          style={{fontSize: '50px', color: 'red', float: 'left', fontWeight: '900'}}><Icon
          type="check-circle-o"/></span><span
          style={{fontSize: '30px', color: '#000', marginLeft: 20, float: 'left', fontWeight: '900'}}>注册成功</span></div>
        <div style={{fontSize: '18px', color: '#8a92a3', marginTop: 20}}>请在24小时内通过邮箱激活您的账号。</div>
        <Link to="sendemail"><Button type="primary" htmlType="submit" className={styles.login}>
          邮箱激活
        </Button></Link>
      </Form>
    </div>
  )
}
Sucesssigiter = connect((state) => {
  return {
    ...state.emailregister,
  }
})(Sucesssigiter);

export default create()(Sucesssigiter);
