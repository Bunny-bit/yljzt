import React from 'react';
import {
	Modal,
	Form,
	Input,
	Button,
	Row,
	Col,
	Icon,
	Select,
	Radio,
	Tabs,
	Checkbox,
	Upload,
	message,
	Button,
	Icon
} from 'antd';
const create = Form.create;
const FormItem = Form.Item;
import FormUpload from '../../../components/FormUpload/FormUpload';
import { remoteUrl } from '../../../utils/url';
const { TextArea } = Input;

import { connect } from 'dva';

function AppEditionModal({ dispatch, form, modalVisible, modalText, isAdd, isIOS, record }) {
	function handleCancel() {
		dispatch({
			type: 'appEdition/setState',
			payload: {
				modalVisible: false
			}
		});
	}

	const { getFieldDecorator, getFieldValue } = form;

	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};
	const checkCol = {
		labelCol: { span: 8 },
		wrapperCol: { offset: 8, span: 12 }
	};

	return (
		<Modal
			visible={modalVisible}
			title={modalText}
			onCancel={handleCancel}
			footer={[
				<Button
					key="back"
					type="primary"
					size="large"
					onClick={() => {
						form.validateFields((err, values) => {
							console.log(err, values);
							if (!err) {
								if (!isAdd) {
									values.id = record.id;
								}
								dispatch({
									type: 'appEdition/createOrUpdate',
									payload: values
								});
							}
						});
					}}
				>
					保存
				</Button>,
				<Button key="submit" size="large" onClick={handleCancel}>
					取消
				</Button>
			]}
		>
			<Form>
				<FormItem label="版本号" {...formCol}>
					{getFieldDecorator('version', {
						initialValue: record.version,
						rules: [ { required: true, message: '请填写版本号' } ]
					})(<Input placeholder="请输入版本号" />)}
				</FormItem>
				<FormItem label="关于连接地址" {...formCol}>
					{getFieldDecorator('aboutUrl', {
						initialValue: record.aboutUrl,
						rules: [ { type: 'url', required: true, message: '请填写正确的关于连接地址' } ]
					})(<Input placeholder="请输入关于连接地址" />)}
				</FormItem>
				<FormItem {...checkCol}>
					{getFieldDecorator('isMandatoryUpdate', {
						valuePropName: 'checked',
						initialValue: record.isMandatoryUpdate
					})(<Checkbox style={{ color: '#000' }}>是否强制更新</Checkbox>)}
				</FormItem>
				<FormItem {...checkCol}>
					{getFieldDecorator('isActive', {
						valuePropName: 'checked',
						initialValue: record.isActive
					})(<Checkbox style={{ color: '#000' }}>是否启用</Checkbox>)}
				</FormItem>

				<FormItem label="安装包" {...formCol}>
					{getFieldDecorator('installationPackage', {
						initialValue: record.installationPackage
					})(<FormUpload action={remoteUrl + '/api/services/app/appEditions/UploadAppEdition'} />)}
				</FormItem>

				<FormItem label="描述" {...formCol} help="例：“修复了一系列bug，升级了很多功能。”">
					{getFieldDecorator('describe', {
						initialValue: record.describe
					})(<TextArea placeholder="请输入描述" autosize />)}
				</FormItem>

				{isIOS ? (
					<FormItem label="Itunes连接地址" {...formCol}>
						{getFieldDecorator('itunesUrl', {
							initialValue: record.itunesUrl,
							rules: [ { type: 'url', message: '请填写正确的Itunes连接地址' } ]
						})(<Input placeholder="请输入Itunes连接地址" />)}
					</FormItem>
				) : null}
			</Form>
		</Modal>
	);
}

AppEditionModal = connect((state) => {
	return {
		...state.appEdition
	};
})(AppEditionModal);

export default create()(AppEditionModal);
