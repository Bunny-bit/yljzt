import React from 'react';
import { Form, Icon } from 'antd';
const create = Form.create;
import { connect } from 'dva';

class Geetest extends React.Component {
	state = { loading: true };

	static defaultProps = {
		captchaHtmlId: '#embed-captcha'
	};

	componentDidMount() {
		this.props.dispatch({
			type: 'geetest/getCaptcha',
			payload: {
				callback: (data) => {
					initGeetest(
						{
							gt: data.gt,
							challenge: data.challenge,
							product: 'embed', // 产品形式，包括：float，embed，popup。注意只对PC版验证码有效
							offline: !data.success, // 表示用户后台检测极验服务器是否宕机，一般不需要关注
							new_captcha: data.new_captcha,
							width: '100%'
							// 更多配置参数请参见：http://www.geetest.com/install/sections/idx-client-sdk.html#config
						},
						(captchaObj) => {
							this.setState({ loading: false });
							captchaObj.props = this.props;
							window.captchaObj = captchaObj;
							captchaObj.appendTo('#' + this.props.captchaHtmlId);
							captchaObj.onSuccess(() => {
								var result = captchaObj.getValidate();
								this.props.onChange(
									JSON.stringify({
										challenge: result.geetest_challenge,
										validate: result.geetest_validate,
										seccode: result.geetest_seccode
									})
								);
							});
						}
					);
				}
			}
		});
	}

	render() {
		return <div>{this.state.loading ? <div>正在加载验证码......</div> : <div id={this.props.captchaHtmlId} />}</div>;
	}
}

Geetest = Form.create()(Geetest);

export default connect((state) => {
	return {
		...state.geetest
	};
})(Geetest);
