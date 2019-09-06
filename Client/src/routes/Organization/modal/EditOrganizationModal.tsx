import React from 'react';
import styles from './../OrganizationList.css';
import { Modal, Form, Input, Button, Row, Col, Icon, Select, Radio } from 'antd';
const create = Form.create;
const FormItem = Form.Item;
const Option = Select.Option;
const RadioGroup = Radio.Group;

import { connect } from 'dva';

/**
 * EditOrganizationModal.js
 * Created by 凡尧 on 2017/9/12 15:59
 * 描述: 编辑组织机构弹窗
 */
function EditOrganizationModal({
	dispatch,
	form,
	editOrganizationModalVisible,
	organization,
	parentId,
	editOrganizationId,
	isAddOrganization
}) {
	function handleCancel() {
		dispatch({
			type: 'organization/setState',
			payload: {
				editOrganizationModalVisible: !editOrganizationModalVisible
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
			visible={editOrganizationModalVisible}
			width={658}
			title={isAddOrganization ? '添加组织机构' : '编辑组织机构'}
			onCancel={handleCancel}
			footer={[
				<Button
					key="back"
					type="primary"
					onClick={() => {
						form.validateFields((err, values) => {
							let data = {
								id: organization.id,
								displayName: values.displayName,
								parentId: parentId
							};
							if (!err) {
								if (isAddOrganization) {
									dispatch({
										type: 'organization/createOrganizationUnit',
										payload: {
											...data
										}
									});
								} else {
									dispatch({
										type: 'organization/updateOrganizationUnit',
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
				<FormItem label="组织机构名称" {...formCol}>
					{getFieldDecorator('displayName', {
						initialValue: isAddOrganization ? '' : organization.displayName,
						rules: [ { required: true, message: '请填写组织机构名称' } ]
					})(<Input placeholder="请输入组织机构名称" />)}
				</FormItem>
			</Form>
		</Modal>
	);
}

EditOrganizationModal = connect((state) => {
	return {
		...state.organization
	};
})(EditOrganizationModal);

export default create()(EditOrganizationModal);
