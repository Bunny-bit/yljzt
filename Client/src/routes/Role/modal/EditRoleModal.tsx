import React from 'react';
import styles from './../Role.css';
import { Modal, Form, Input, Button, Row, Col, Icon, Select, Radio, Tabs, Checkbox, Tree } from 'antd';
const create = Form.create;
const FormItem = Form.Item;
const TabPane = Tabs.TabPane;
const TreeNode = Tree.TreeNode;

import { connect } from 'dva';

/**
 * EditRoleModal.js
 * Created by 凡尧 on 2017/9/12 15:59
 * 描述: 添加角色弹窗
 */
function EditRoleModal({
	dispatch,
	form,
	editRoleModalState,
	permissionTree,
	selectRoles,
	editingRole,
	isInitializingModal
}) {
	function handleCancel() {
		dispatch({
			type: 'role/setState',
			payload: {
				editRoleModalState: { ...editRoleModalState, visible: false }
			}
		});
	}

	const { getFieldDecorator, getFieldValue } = form;

	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};

	const loopPermissions = (data) =>
		data.map((item) => {
			if (item.children && item.children.length) {
				return (
					<TreeNode key={item.name} title={item.displayName}>
						{loopPermissions(item.children)}
					</TreeNode>
				);
			}
			return <TreeNode key={item.name} title={item.displayName} />;
		});

	function onCheck(checkedKeys, info) {
		dispatch({
			type: 'role/setState',
			payload: {
				selectRoles: checkedKeys
			}
		});
	}

	function onTabChange(value) {
		dispatch({
			type: 'role/setState',
			payload: {
				editRoleModalState: { ...editRoleModalState, activeTabKey: value }
			}
		});
	}

	return (
		<Modal
			visible={editRoleModalState.visible}
			title="修改角色"
			onCancel={handleCancel}
			footer={[
				<Button
					key="back"
					type="primary"
					onClick={() => {
						form.validateFields((err, values) => {
							let data = {
								role: {
									id: editingRole.role.id,
									displayName: values.displayName,
									isDefault: values.isDefault
								},
								grantedPermissionNames: selectRoles
							};
							if (!err) {
								dispatch({
									type: 'role/createOrUpdateRole',
									payload: data
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
			<Tabs activeKey={editRoleModalState.activeTabKey} onChange={onTabChange}>
				<TabPane tab="角色属性" key="1">
					<Form>
						<FormItem label="角色名称" {...formCol}>
							{getFieldDecorator('displayName', {
								initialValue: editingRole.role.displayName,
								rules: [ { required: true, message: '请填写角色名称' } ]
							})(<Input placeholder="请输入角色名称" />)}
						</FormItem>
						<FormItem {...formCol} className={styles.showbox}>
							{getFieldDecorator('isDefault', {
								valuePropName: 'checked',
								initialValue: editingRole.role.isDefault
							})(<Checkbox style={{ color: '#000' }}>设置为默认角色</Checkbox>)}
							<div style={{ marginTop: '20px', width: '130%' }}>新增用户将会默认拥有此角色</div>
						</FormItem>
					</Form>
				</TabPane>
				<TabPane tab="操作权限" key="2">
					<Tree checkable onCheck={onCheck} defaultCheckedKeys={selectRoles}>
						{loopPermissions(permissionTree)}
					</Tree>
				</TabPane>
			</Tabs>
		</Modal>
	);
}

EditRoleModal = connect((state) => {
	return {
		...state.role
	};
})(EditRoleModal);

export default create()(EditRoleModal);
