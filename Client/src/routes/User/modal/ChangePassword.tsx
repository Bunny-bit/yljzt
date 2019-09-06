import React from 'react';
import styles from './../User.css';
import { Form, Checkbox, Input, Button, Row, Col, Icon, Badge, Tabs, Modal, Tooltip, Tree } from 'antd';
const create = Form.create;
const FormItem = Form.Item;
const TabPane = Tabs.TabPane;
const Search = Input.Search;
const TreeNode = Tree.TreeNode;

import { connect } from 'dva';

function ChangePassword({
	dispatch,
	pagination,
	form,
	loading,
	changePasswordModalState,
	roles,
	selectedOrganizations
}) {
	function handleCancel() {
		dispatch({
			type: 'user/setState',
			payload: {
				changePasswordModalState: { ...changePasswordModalState, visible: false }
			}
		});
		form.resetFields();
	}

	const { getFieldDecorator, getFieldValue } = form;

	function handleConfirmPassword(rule, value, callback) {
		if (value && value !== form.getFieldValue('newPassword')) {
			callback('两次输入不一致！');
		}

		// Note: 必须总是返回一个 callback，否则 validateFieldsAndScroll 无法响应
		callback();
	}

	return (
		<Modal
			visible={changePasswordModalState.visible}
			width={658}
			title="修改用户密码"
			onCancel={handleCancel}
			footer={[
				<Button
					key="back"
					type="primary"
					loading={loading}
					htmlType="submit"
					onClick={() => {
						form.validateFields((err, values) => {
							dispatch({
								type: 'user/changeUserPassword',
								payload: {
									id: changePasswordModalState.user.id,
									name: changePasswordModalState.user.name,
									newPassword: values.newPassword
								}
							});

							form.resetFields();
						});
					}}
				>
					提交
				</Button>,
				<Button key="submit" onClick={handleCancel}>
					取消
				</Button>
			]}
		>
			<Form>
				<FormItem label="新密码：" labelCol={{ span: 6 }} wrapperCol={{ span: 15 }}>
					{getFieldDecorator('newPassword', {
						initialValue: '',
						rules: [ { required: true, message: '请填写新密码' } ]
					})(<Input type="password" />)}
				</FormItem>
				<FormItem label="确认密码：" labelCol={{ span: 6 }} wrapperCol={{ span: 15 }}>
					{getFieldDecorator('newPassword1', {
						initialValue: '',
						rules: [
							{ required: true, message: '请填写确认密码' },
							{
								validator: handleConfirmPassword
							}
						]
					})(<Input type="password" />)}
				</FormItem>
			</Form>
		</Modal>
	);
}

ChangePassword = connect((state) => {
	return {
		...state.user,
		loading: state.loading.effects['user/createOrUpdateUser']
	};
})(ChangePassword);

export default create()(ChangePassword);
