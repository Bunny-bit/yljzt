import React from 'react';
import { Modal, Input, Button, Row, Col, Icon, Form } from 'antd';
const create = Form.create;
const FormItem = Form.Item;
import { connect } from 'dva';

/**
 * BindingModal.js
 * Created by 凡尧 on 2017/10/17
 * 描述: 绑定账号弹窗
 */
function BindingModal({ dispatch, bindingModalVisible, form, thirdPartyToken }) {
	function handleSubmit() {
		form.validateFields((err, values) => {
			let data = {
				userName: values.userName,
				password: values.password,
				token: thirdPartyToken
			};
			if (!err) {
				dispatch({
					type: 'thirdpartybinding/bindingThirdParty',
					payload: {
						...data
					}
				});
			}
		});
	}

	function handleCancel() {
		dispatch({
			type: 'thirdpartybinding/setState',
			payload: {
				bindingModalVisible: !bindingModalVisible
			}
		});
	}

	const { getFieldDecorator, getFieldValue } = form;

	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};

	return (
		<Modal
			visible={bindingModalVisible}
			width={658}
			title="绑定账号"
			onCancel={handleCancel}
			footer={[
				<Button key="submit" type="primary" onClick={handleSubmit}>
					绑定
				</Button>,
				<Button key="cancel" onClick={handleCancel}>
					取消
				</Button>
			]}
		>
			<Form>
				<FormItem label="登录名" {...formCol}>
					{getFieldDecorator('userName', {
						initialValue: '',
						rules: [ { required: true, message: '请填写登录名' } ]
					})(<Input placeholder="请输入登录名" />)}
				</FormItem>
				<FormItem label="登录密码" {...formCol}>
					{getFieldDecorator('password', {
						initialValue: '',
						rules: [ { required: true, message: '请填写登录密码' } ]
					})(<Input placeholder="请输入登录密码" type="password" />)}
				</FormItem>
			</Form>
		</Modal>
	);
}

BindingModal = connect((state) => {
	return {
		...state.thirdpartybinding,
		thirdPartyToken: state.indexpage.thirdPartyToken
	};
})(BindingModal);

export default create()(BindingModal);
