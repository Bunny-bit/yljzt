import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button} from 'antd';
import styles from './Activation.css';
const FormItem = Form.Item;
const create = Form.create;
function Sendemail({dispatch, form}) {
  const {getFieldDecorator} = form;

  function handleSubmit(e) {
    e.preventDefault();
    form.validateFields(['email'], (err, values) => {
      if (!err) {
        dispatch({
          type: 'sendemail/sentpass',
          payload: values
        });
      }
    })
  }

  return (
    <div>
      <Form onSubmit={handleSubmit} className={`${styles.formbox} login-form`}>
        <FormItem>
          <div className={styles.colorsize}>邮箱地址：</div>
          {getFieldDecorator('email', {
            rules: [{required: true, message: '请输入邮箱地址！'}],
          })(
            <Input prefix={<Icon type="mail"/>} placeholder="请输入邮箱地址"
                   style={{fontSize: 13, marginTop: '10px', height: 40}}/>
          )}
        </FormItem>
        <p style={{color: '#8a92a3', fontSize: '18px'}}>系统会立即向您发送一封邮件用于激活您的账号，请接收并点击邮件内容中的激活链接，如果在两分钟内还没有收到激活邮件，请重试。</p>
        <Button type="primary" htmlType="submit" className={styles.login}>
          发送激活邮件
        </Button>
      </Form>
    </div>
  )
}
Sendemail = connect((state) => {
  return {
    ...state.Sendemail,
  }
})(Sendemail);

export default create()(Sendemail);
