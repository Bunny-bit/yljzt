import React from 'react';
import styles from './../Role.css';
import { Modal, Form, Input, Button, Row, Col, Icon, Select, Radio, Tabs, Checkbox, Tree } from 'antd';
const create = Form.create;
const FormItem = Form.Item;
const TabPane = Tabs.TabPane;
const TreeNode = Tree.TreeNode;

import { connect } from 'dva';

/**
 * AddRoleModal.js
 * Created by 凡尧 on 2017/9/12 15:59
 * 描述: 添加角色弹窗
 */
function AddRoleModal({ dispatch, form, addRoleModalState, permissionTree, selectRoles }) {
	function handleCancel() {
		dispatch({
			type: 'role/setState',
			payload: {
				addRoleModalState: { ...addRoleModalState, visible: false }
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
			payload: { addRoleModalState: { ...addRoleModalState, activeTabKey: value } }
		});
	}

	return (
		<Modal
			visible={addRoleModalState.visible}
			title="添加角色"
			onCancel={handleCancel}
			footer={[
				<Button
					key="back"
					type="primary"
					onClick={() => {
						form.validateFields((err, values) => {
							let data = {
								role: { displayName: values.displayName, isDefault: values.isDefault },
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
			<Tabs activeKey={addRoleModalState.activeTabKey} onChange={onTabChange}>
				<TabPane tab="角色属性" key="1">
					<Form>
						<FormItem label="角色名称" {...formCol}>
							{getFieldDecorator('displayName', {
								rules: [ { required: true, message: '请填写角色名称' } ]
							})(<Input placeholder="请输入角色名称" />)}
						</FormItem>
						<FormItem {...formCol} className={styles.showbox}>
							{getFieldDecorator('isDefault', {
								valuePropName: 'checked',
								initialValue: true
							})(
								<Checkbox defaultChecked={true} style={{ color: '#000' }}>
									设置为默认角色
								</Checkbox>
							)}
							<div style={{ marginTop: '20px', width: '130%' }}>新增用户将会默认拥有此角色</div>
						</FormItem>
					</Form>
				</TabPane>
				<TabPane tab="操作权限" key="2">
					<Tree checkable checkedKeys={selectRoles} onCheck={onCheck}>
						{loopPermissions(permissionTree)}
					</Tree>
				</TabPane>
			</Tabs>
		</Modal>
	);
}

AddRoleModal = connect((state) => {
	return {
		...state.role
	};
})(AddRoleModal);

export default create()(AddRoleModal);
