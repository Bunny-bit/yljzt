import React from 'react';
import styles from './../User.css';
import { Modal, Button, Row, Col, Icon, Select, Radio, Tabs, Checkbox, Tree } from 'antd';
const TabPane = Tabs.TabPane;
const TreeNode = Tree.TreeNode;

import { connect } from 'dva';

/**
 * ChangeUserPermission.js
 * Created by 凡尧 on 2017/9/19 15:59
 * 描述: 修改用户权限弹窗
 */
function ChangeUserPermission({
	dispatch,
	currentUser,
	currentEditUserPermission,
	changeUserPermissionModalState,
	selectPermission
}) {
	function handleCancel() {
		dispatch({
			type: 'user/setState',
			payload: {
				changeUserPermissionModalState: { ...changeUserPermissionModalState, visible: false }
			}
		});
	}

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
			type: 'user/setState',
			payload: {
				selectPermission: checkedKeys
			}
		});
	}

	function resetUserSpecificPermissions() {
		dispatch({
			type: 'user/resetUserSpecificPermissions',
			payload: {
				id: currentUser.id
			}
		});
	}

	return (
		<Modal
			visible={changeUserPermissionModalState.visible}
			title={'修改权限 - ' + (currentUser ? currentUser.name : '')}
			onCancel={handleCancel}
			footer={[
				<Button
					key="back"
					type="primary"
					onClick={() => {
						let data = {
							id: currentUser.id,
							grantedPermissionNames: selectPermission
						};
						dispatch({
							type: 'user/updateUserPermissions',
							payload: data
						});
					}}
				>
					保存
				</Button>,
				<Button key="submit" onClick={handleCancel}>
					取消
				</Button>,
				<Button key="reset" onClick={resetUserSpecificPermissions}>
					恢复默认权限
				</Button>
			]}
		>
			<Tabs defaultActiveKey="1">
				<TabPane tab="操作权限" key="1">
					<Tree checkable onCheck={onCheck} checkedKeys={selectPermission}>
						{loopPermissions(
							currentEditUserPermission.permissions ? currentEditUserPermission.permissions : []
						)}
					</Tree>
				</TabPane>
			</Tabs>
		</Modal>
	);
}

ChangeUserPermission = connect((state) => {
	return {
		...state.user
	};
})(ChangeUserPermission);

export default ChangeUserPermission;
