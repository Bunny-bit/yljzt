import React from 'react';
import { remoteUrl } from './../../utils/url';
import { Form, Button, Row, Col, Icon, Tabs, Upload, Modal, Checkbox, Input, Radio, InputNumber } from 'antd';
const create = Form.create;
const FormItem = Form.Item;
const TabPane = Tabs.TabPane;
import Setting from './../../components/Setting/Setting';

import { connect } from 'dva';

function Configuration({ dispatch, allSetting, form, mode, passwordComplexitySetting }) {
	const { getFieldDecorator } = form;
	const props = {
		name: 'file',
		action: `${remoteUrl}/Logo/UploadLogoPicture`,
		showUploadList: false,
		headers: {
			authorization: 'Bearer ' + window.localStorage.getItem('token')
		},
		beforeUpload(file) {
			var isTypeTrue = file.type === 'image/jpeg' || file.type === 'image/png';
			if (!isTypeTrue) {
				Modal.warning({
					title: '请上传JPG/PNG类型的文件'
				});
			}
			return isTypeTrue;
		},
		onChange(info) {
			if (info.file.status !== 'uploading') {
				console.log(info.file, info.fileList);
			}
			if (info.file.status === 'done') {
				if (info.fileList[info.fileList.length - 1].response.error) {
					Modal.error({
						title: '上传失败',
						content: info.fileList[info.fileList.length - 1].response.error.message
					});
					return;
				}
				Modal.success({
					title: '上传成功'
				});
			} else if (info.file.status === 'error') {
				Modal.error({
					title: '上传失败'
				});
			}
		}
	};

	const handleModeChange = (e) => {
		const mode = e.target.value;
		dispatch({
			type: 'configuration/setState',
			payload: { mode: mode }
		});
		if (mode == 'default') {
			form.setFieldsValue({
				MinLength: 6,
				MaxLength: 10,
				UseNumbers: true,
				UseUpperCaseLetters: false,
				UseLowerCaseLetters: true,
				UsePunctuations: false
			});
		}
	};

	let disabledOptions = { disabled: mode == 'default' };

	return (
		<div>
			<Button
				type="primary"
				onClick={() => {
					form.validateFields((err, values) => {
						if (!err) {
							var payload = values;
							if (payload.PasswordComplexity) {
								payload.SecuritySettingDto.PasswordComplexity = payload.PasswordComplexity;
								payload.SecuritySettingDto.UseDefaultPasswordComplexity = mode == 'default';
								delete payload.PasswordComplexity;
							}

							dispatch({
								type: 'configuration/updateAllSettings',
								payload: payload
							});
						}
					});
				}}
			>
				保存设置
			</Button>
			<Tabs>
				{allSetting.map((setting) => (
					<TabPane tab={setting.tabName} key={setting.name} forceRender={true}>
						{setting.name == 'SystemSettingDto' ? (
							<div>
								<h2>应用LOGO</h2>
								<br />
								<Upload {...props}>
									<Button type="ghost">
										<Icon type="upload" />点击上传
									</Button>
								</Upload>
								<div style={{ color: '#8A92A3', marginTop: '12px' }}>
									请上传JPG/PNG类型的文件，图片小于30KB，图片尺寸为168*46
								</div>
								<br />
								<div style={{ width: '300px' }}>
									{getFieldDecorator(setting.name)(
										<Setting data={setting.setting.filter((s) => s.name != 'SystemSettingDto')} />
									)}
								</div>
							</div>
						) : setting.name == 'SecuritySettingDto' ? (
							<div>
								<Form layout="horizontal" className="ant-advanced-search-form">
									<h2>密码复杂性</h2>
									<br />
									密码组成：
									<Radio.Group onChange={handleModeChange} value={mode} style={{ marginBottom: 8 }}>
										<Radio.Button value="default">默认设置</Radio.Button>
										<Radio.Button value="custom">自定义设置</Radio.Button>
									</Radio.Group>
									<div>
										<FormItem>
											{getFieldDecorator('PasswordComplexity.UseNumbers', {
												valuePropName: 'checked',
												initialValue: passwordComplexitySetting.UseNumbers
											})(<Checkbox {...disabledOptions}>需要数字</Checkbox>)}
										</FormItem>
										<FormItem>
											{getFieldDecorator('PasswordComplexity.UseUpperCaseLetters', {
												valuePropName: 'checked',
												initialValue: passwordComplexitySetting.UseUpperCaseLetters
											})(<Checkbox {...disabledOptions}>需要大写字母</Checkbox>)}
										</FormItem>
										<FormItem>
											{getFieldDecorator('PasswordComplexity.UseLowerCaseLetters', {
												valuePropName: 'checked',
												initialValue: passwordComplexitySetting.UseLowerCaseLetters
											})(<Checkbox {...disabledOptions}>需要小写字母</Checkbox>)}
										</FormItem>
										<FormItem>
											{getFieldDecorator('PasswordComplexity.UsePunctuations', {
												valuePropName: 'checked',
												initialValue: passwordComplexitySetting.UsePunctuations
											})(<Checkbox {...disabledOptions}>需要英文符号</Checkbox>)}
										</FormItem>
										<FormItem label="密码长度要求">
											<Row>
												<Col span={1}>
													<FormItem>
														{getFieldDecorator('PasswordComplexity.MinLength', {
															initialValue: passwordComplexitySetting.MinLength
														})(<InputNumber {...disabledOptions} />)}
													</FormItem>
												</Col>
												<Col span={1}>
													<span
														style={{
															display: 'inline-block',
															width: '100%',
															textAlign: 'center'
														}}
													>
														-
													</span>
												</Col>
												<Col span={1}>
													<FormItem>
														{getFieldDecorator('PasswordComplexity.MaxLength', {
															initialValue: passwordComplexitySetting.MaxLength
														})(<InputNumber {...disabledOptions} />)}
													</FormItem>
												</Col>
											</Row>
										</FormItem>
									</div>
								</Form>
								<div style={{ width: '300px' }}>
									{getFieldDecorator(setting.name)(
										<Setting data={setting.setting.filter((s) => s.name != 'PasswordComplexity')} />
									)}
								</div>
							</div>
						) : (
							<div style={{ width: '300px' }}>
								{getFieldDecorator(setting.name)(<Setting data={setting.setting} />)}
							</div>
						)}
					</TabPane>
				))}
			</Tabs>
		</div>
	);
}

Configuration = connect((state) => {
	return {
		...state.configuration
	};
})(Configuration);

export default create()(Configuration);
