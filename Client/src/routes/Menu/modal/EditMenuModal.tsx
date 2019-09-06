import React from 'react';
import styles from './../MenuList.css';
import IconSelector from '../../../components/IconSelector/IconSelector';
import { Modal, Form, Input, Button, Row, Col, Icon, Select, Radio, TreeSelect } from 'antd';
const create = Form.create;
const FormItem = Form.Item;
const Option = Select.Option;
const RadioGroup = Radio.Group;
const TreeNode = TreeSelect.TreeNode;

import { connect } from 'dva';

/**
 * EditMenuModal.js
 * Created by 凡尧 on 2017/9/12 15:59
 * 描述: 编辑菜单弹窗
 */
function EditMenuModal({ dispatch, form, isEditingMenu, editMenuModalVisible, menu, parentId, permissions }) {
	function handleCancel() {
		dispatch({
			type: 'menu/setState',
			payload: {
				editMenuModalVisible: !editMenuModalVisible
			}
		});
	}

	const { getFieldDecorator, getFieldValue } = form;
	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};

	const permissionOptions = (data) =>
		data.map((permission) => {
			return (
				<Option value={permission.name}>
					{permission.displayName}[{permission.name}]
				</Option>
			);
		});

	const loopPermissions = (data) =>
		data.map((item) => {
			if (item.children && item.children.length) {
				return (
					<TreeNode value={item.name} key={item.name} title={item.displayName}>
						{loopPermissions(item.children)}
					</TreeNode>
				);
			}
			return <TreeNode value={item.name} key={item.name} title={item.displayName} />;
		});

	return (
		<Modal
			visible={editMenuModalVisible}
			width={658}
			title={<div style={{ fontSize: 20, color: '#000' }}>{isEditingMenu ? '编辑菜单' : '添加菜单'}</div>}
			onCancel={handleCancel}
			footer={[
				<Button
					key="back"
					type="primary"
					onClick={() => {
						form.validateFields((err, values) => {
							let data = {
								id: menu.name,
								displayName: values.displayName,
								url: values.url,
								isVisible: values.isVisible,
								RequiredPermissionName: values.requiredPermissionName,
								parentId: parentId,
								icon: values.icon
							};
							if (!err) {
								if (menu.name) {
									dispatch({
										type: 'menu/updateMenu',
										payload: {
											...data
										}
									});
								} else {
									dispatch({
										type: 'menu/createCustomMenu',
										payload: {
											...data
										}
									});
								}
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
			<Form>
				<FormItem label="菜单名称" {...formCol}>
					{getFieldDecorator('displayName', {
						initialValue: isEditingMenu ? menu.displayName : '',
						rules: [ { required: true, message: '请填写菜单名称' } ]
					})(<Input placeholder="请输入菜单名称" />)}
				</FormItem>
				<FormItem label="链接地址" {...formCol}>
					{getFieldDecorator('url', {
						initialValue: isEditingMenu ? menu.url : null,
						rules: [ { required: true, message: '请填写菜单链接地址' } ]
					})(<Input placeholder="请输入菜单链接地址" disabled={menu.isSystem} />)}
				</FormItem>
				<FormItem label="权限" {...formCol}>
					{getFieldDecorator('requiredPermissionName', {
						initialValue: isEditingMenu ? menu.requiredPermissionName : ''
					})(
						<TreeSelect
							dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
							placeholder="选择权限"
							allowClear
							treeDefaultExpandAll
						>
							{loopPermissions(permissions)}
						</TreeSelect>
					)}
				</FormItem>
				<FormItem label="菜单图标" {...formCol}>
					{getFieldDecorator('icon', {
						initialValue: isEditingMenu ? menu.icon : null
					})(<IconSelector />)}
				</FormItem>
				<FormItem label="是否显示" {...formCol}>
					{getFieldDecorator('isVisible', {
						initialValue: isEditingMenu ? (menu.isVisible ? 'true' : 'false') : 'true',
						rules: [ { required: true, message: '请填写菜单链接地址' } ]
					})(
						<RadioGroup>
							<Radio value="true">显示</Radio>
							<Radio value="false">隐藏</Radio>
						</RadioGroup>
					)}
				</FormItem>
			</Form>
		</Modal>
	);
}

EditMenuModal = connect((state) => {
	return {
		...state.menu
	};
})(EditMenuModal);

export default create()(EditMenuModal);
