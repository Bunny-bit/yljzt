import React from 'react';
import { Form, Checkbox, Input, Button, Row, Col, Icon, Badge, Modal, Tooltip } from 'antd';
import { connect } from 'dva';
import { remoteUrl } from '../../../utils/url';

function BindingThirdParty({ dispatch, bindingThirdPartyModalVisible, thirdPartyList }) {
	function handleCancel() {
		dispatch({
			type: 'thirdpartybinding/setState',
			payload: {
				bindingThirdPartyModalVisible: !bindingThirdPartyModalVisible
			}
		});
	}

	function bindingThirdParty(url) {
		location = url;
	}

	function unbindingThirdParty(thirdParty) {
		dispatch({
			type: 'thirdpartybinding/loginUserUnbindingThirdParty',
			payload: {
				thirdParty: thirdParty.thirdParty,
				thirdPartyName: thirdParty.thirdPartyName
			}
		});
	}

	return (
		<Modal
			visible={bindingThirdPartyModalVisible}
			width={600}
			title="第三方账号绑定"
			onCancel={handleCancel}
			footer={null}
		>
			<div>
				<p>绑定第三方账号，可以直接登录系统</p>
				<p>如果当前帐号在对应站点处于登录状态，需退出登录后，才能重新绑定</p>
				{thirdPartyList.map((data, i) => {
					return (
						<Row style={{ marginTop: 10 }} key={i}>
							<Col span={2}>
								<img
									src={`${remoteUrl}` + data.iconUrl}
									style={{ marginLeft: 10, height: 20, width: 20 }}
								/>
							</Col>
							<Col span={3}>
								<p>{data.thirdPartyName}</p>
							</Col>
							<Col span={3}>
								<p>{data.isBinding ? '已绑定' : '未绑定'}</p>
							</Col>
							<Col span={8}>
								{data.isBinding ? (
									<Button
										onClick={unbindingThirdParty.bind(this, {
											thirdParty: data.thirdParty,
											thirdPartyName: data.thirdPartyName
										})}
									>
										解绑
									</Button>
								) : (
									<Button onClick={bindingThirdParty.bind(this, data.authUrl)} type="primary">
										绑定
									</Button>
								)}
							</Col>
						</Row>
					);
				})}
			</div>
		</Modal>
	);
}

export default connect((state) => {
	return {
		...state.thirdpartybinding
	};
})(BindingThirdParty);
