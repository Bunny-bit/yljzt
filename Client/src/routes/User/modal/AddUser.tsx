import React from 'react';
import styles from './../User.css';
import { Form, Checkbox, Input, Button, Row, Col, Icon, Badge, Tabs, Modal, Tooltip, Tree, message } from 'antd';
const create = Form.create;
const FormItem = Form.Item;
const TabPane = Tabs.TabPane;
const Search = Input.Search;
const TreeNode = Tree.TreeNode;
import SearchTree from './../module/SearchTree';

import { connect } from 'dva';

/**
 * User.js
 * Created by 李廷旭 on 2017/9/6 9:30
 * 描述: 用户管理界面
 */
function AddUser({ dispatch, pagination, form, loading, addUserModalState, roles, selectedOrganizations }) {
	function handleCancel() {
		dispatch({
			type: 'user/setState',
			payload: {
				addUserModalState: { ...addUserModalState, visible: false }
			}
		});
	}

	const { getFieldDecorator, getFieldValue } = form;
	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};
	const formCol2 = {
		wrapperCol: { span: 12, offset: 8 }
	};
	const formCol3 = {
		wrapperCol: { span: 12 }
	};

	function onTabChange(value) {
		dispatch({
			type: 'user/setState',
			payload: { addUserModalState: { ...addUserModalState, activeTabKey: value } }
		});
	}

	function getSelectedRoleCount() {
		var values = form.getFieldsValue();
		var count = 0;
		for (let key in values) {
			if (key.indexOf('role_') != -1) {
				if (values[key]) {
					count++;
				}
			}
		}
		return count;
	}

	return (
		<Modal
			visible={addUserModalState.visible}
			width={658}
			title="添加用户"
			onCancel={handleCancel}
			footer={[
				<Button
					key="back"
					type="primary"
					loading={loading}
					htmlType="submit"
					onClick={() => {
						form.validateFields((err, values) => {
							if (values.emailAddress == '' && values.phoneNumber == '') {
								message.error('手机号和邮箱不能同时为空');
								return;
							}
							let assignedRoleNames = [];
							//处理角色
							for (let key in values) {
								if (key.indexOf('role_') != -1) {
									if (values[key]) {
										assignedRoleNames.push(key.split('_')[1]);
									}
								}
							}
							let data = {
								user: {
									name: values.name,
									surname: 'surname',
									userName: values.userName,
									password: values.password,
									emailAddress: values.emailAddress,
									phoneNumber: values.phoneNumber,
									isActive: values.isActive,
									shouldChangePasswordOnNextLogin: values.shouldChangePasswordOnNextLogin,
									isTwoFactorEnabled: values.isTwoFactorEnabled,
									isLockoutEnabled: values.isLockoutEnabled
								},
								assignedRoleNames: assignedRoleNames,
								sendActivationEmail: values.sendActivationEmail,
								sendActivationMessage: values.sendActivationEmail,
								setRandomPassword: values.setRandomPassword,
								organizations: selectedOrganizations
							};
							if (!err) {
								dispatch({
									type: 'user/createOrUpdateUser',
									payload: {
										...data,
										pagination
									}
								});
							}
						});
					}}
				>
					保存
				</Button>,
				<Button key="submit" onClick={handleCancel}>
					取消
				</Button>
			]}
		>
			<Tabs activeKey={addUserModalState.activeTabKey} onChange={onTabChange}>
				<TabPane tab="基础信息" key="1">
					<Form>
						<FormItem label="姓名" {...formCol}>
							{getFieldDecorator('name', {
								initialValue: '',
								rules: [ { required: true, message: '请填写姓名' } ]
							})(<Input placeholder="请输入" />)}
						</FormItem>
						<FormItem label="邮箱地址" {...formCol}>
							{getFieldDecorator('emailAddress', {
								initialValue: ''
								//rules: [ { type: 'email', required: true, message: '请填写正确的邮箱地址' } ]
							})(<Input placeholder="请输入" />)}
						</FormItem>
						<FormItem label="电话号码" {...formCol}>
							{getFieldDecorator('phoneNumber', {
								initialValue: ''
								//rules: [ { required: true, message: '请填写电话号码' } ]
							})(<Input placeholder="请输入" />)}
						</FormItem>
						<FormItem label="用户名" {...formCol}>
							{getFieldDecorator('userName', {
								initialValue: '',
								rules: [ { required: true, message: '请填写用户' } ]
							})(<Input placeholder="请输入" />)}
						</FormItem>
						<FormItem label="初始密码" {...formCol}>
							{getFieldDecorator('password', {
								initialValue: '',
								rules: [ { required: true, message: '请填写初始密码' } ]
							})(<Input placeholder="请输入" type="password" />)}
						</FormItem>
						{/* <FormItem {...formCol2}>
             {getFieldDecorator('setRandomPassword', {
             valuePropName: 'checked',
             initialValue: true,
             })(
             <Checkbox>使用随机密码</Checkbox>
             )}
             </FormItem> */}
						<FormItem {...formCol2}>
							{getFieldDecorator('shouldChangePasswordOnNextLogin', {
								valuePropName: 'checked',
								initialValue: true
							})(<Checkbox>下次登录需要修改密码</Checkbox>)}
						</FormItem>
						<FormItem {...formCol2}>
							{getFieldDecorator('sendActivationMessage', {
								valuePropName: 'checked',
								initialValue: true
							})(<Checkbox>发送通知短信</Checkbox>)}
						</FormItem>
						<FormItem {...formCol2}>
							{getFieldDecorator('sendActivationEmail', {
								valuePropName: 'checked',
								initialValue: true
							})(<Checkbox>发送通知邮件</Checkbox>)}
						</FormItem>
						<FormItem {...formCol2}>
							{getFieldDecorator('isActive', {
								valuePropName: 'checked',
								initialValue: true
							})(<Checkbox>启用</Checkbox>)}
						</FormItem>
						<FormItem {...formCol2}>
							{getFieldDecorator('isLockoutEnabled', {
								valuePropName: 'checked',
								initialValue: true
							})(<Checkbox>启用锁定</Checkbox>)}
							<Tooltip title="在登录失败一定次数后，用户将会被锁定一段时间。在此期间，用户不可再次尝试登录。">
								<Icon type="question-circle-o" />
							</Tooltip>
						</FormItem>
					</Form>
				</TabPane>
				<TabPane
					tab={
						<div className={styles.mm}>
							<span>角色</span>
							<Badge
								count={getSelectedRoleCount()}
								style={{ backgroundColor: '#5D7DC3', marginLeft: 4 }}
							/>
						</div>
					}
					key="2"
				>
					<Form>
						{roles.map((ele, index) => {
							return (
								<FormItem key={index} {...formCol3}>
									{getFieldDecorator(`role_${ele.name}`, {
										valuePropName: 'checked',
										initialValue: ele.isDefault
									})(<Checkbox>{ele.displayName}</Checkbox>)}
								</FormItem>
							);
						})}
					</Form>
				</TabPane>
				<TabPane tab="组织机构" key="3">
					<Form>
						<SearchTree />
					</Form>
				</TabPane>
			</Tabs>
		</Modal>
	);
}

AddUser = connect((state) => {
	return {
		...state.user,
		loading: state.loading.effects['user/createOrUpdateUser']
	};
})(AddUser);

export default create()(AddUser);
