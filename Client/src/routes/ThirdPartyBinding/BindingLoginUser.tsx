import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Link } from 'dva/router';
import Logo from '../../assets/logologin.gif';
import styles from '../IndexPage.css';
import { remoteUrl, homePageUrl } from '../../utils/url';
import { Icon, Button, notification, Row, Col } from 'antd';
import { routerRedux } from 'dva/router';

class BindingLoginUser extends React.Component {
	componentDidMount() {
		var paramString = decodeURI(location.search);
		var match = /(code|app_auth_code)=([a-zA-Z0-9]*)/;
		if (!match.test(paramString)) {
			this.props.dispatch({
				type: 'thirdpartybinding/setState',
				payload: {
					bindingResult: {
						...this.props.bindingResult,
						isBinding: false,
						message: '未能获取第三方授权码，绑定失败'
					}
				}
			});
			return;
		}
		var routerMatch = /(state|type)=([A-Za-z]*)\|([a-z]*)/;
		if (!routerMatch.test(paramString)) {
			this.props.dispatch({
				type: 'thirdpartybinding/setState',
				payload: {
					bindingResult: {
						...this.props.bindingResult,
						isBinding: false,
						message: '未能获取第三方平台信息，绑定失败'
					}
				}
			});
			return;
		}
		var code = paramString.match(match)[2];
		var type = paramString.match(/(state|type)=([A-Za-z]*)[\||_]([a-z]*)/)[2];
		this.props.dispatch({
			type: 'thirdpartybinding/loginUserBindingThirdParty',
			payload: { ThirdParty: type, Code: code }
		});
	}

	linkToHome = () => {
		location.href = `${location.protocol}//${location.host}/index.html#${homePageUrl}`;
	};

	render() {
		const { bindingResult } = this.props;
		return (
			<div className={styles.navbox}>
				<div className={styles.logologin}>
					<img src={Logo} />
				</div>
				<div className={styles.indexbox}>
					{bindingResult.isBinding ? (
						<div>
							<Row>
								<Col span={3}>
									<Icon type="loading" style={{ fontSize: 30 }} />
								</Col>
								<Col span={20}>
									<p style={{ fontSize: 25, fontWeight: 'bold' }}>正在绑定账号</p>
								</Col>
							</Row>
							<Row style={{ marginTop: 10 }}>
								<Col offset={3}>
									<p>正在为您绑定账号，请您耐心等待！</p>
								</Col>
							</Row>
						</div>
					) : null}
					{!bindingResult.isBinding & bindingResult.success ? (
						<div>
							<Row>
								<Col span={3}>
									<Icon type="check-circle-o" style={{ color: 'green', fontSize: 30 }} />
								</Col>
								<Col span={20}>
									<p style={{ fontSize: 25, fontWeight: 'bold' }}>绑定成功</p>
								</Col>
							</Row>
							<Row style={{ marginTop: 10 }}>
								<Col offset={3}>
									<p>
										您已成功绑定<strong>{bindingResult.platform}</strong>账号
									</p>
								</Col>
							</Row>
							<Row style={{ marginTop: 10 }}>
								<Col offset={3}>
									<Button onClick={this.linkToHome} type="primary">
										返回首页
									</Button>
								</Col>
							</Row>
						</div>
					) : null}
					{!bindingResult.isBinding & !bindingResult.success ? (
						<div>
							<Row>
								<Col span={3}>
									<Icon type="close-circle-o" style={{ color: 'red', fontSize: 30 }} />
								</Col>
								<Col span={20}>
									<p style={{ fontSize: 25, fontWeight: 'bold' }}>绑定失败</p>
								</Col>
							</Row>
							<Row style={{ marginTop: 10 }}>
								<Col offset={3}>
									<p>{bindingResult.message}</p>
								</Col>
							</Row>
							<Row style={{ marginTop: 10 }}>
								<Col offset={3}>
									<Button onClick={this.linkToHome} type="primary">
										返回首页
									</Button>
								</Col>
							</Row>
						</div>
					) : null}
				</div>
			</div>
		);
	}
}

export default connect((state) => {
	return {
		...state.thirdpartybinding
	};
})(BindingLoginUser);
