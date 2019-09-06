import React, { Component } from 'react';
import { connect } from 'dva';
import { Button, message, Tooltip, Modal } from 'antd';

class ReadIDCard extends Component {
	static defaultProps = {
		text: '读取身份证信息',
		errorText: '控件不可用，可能未正确安装控件及驱动，或者控件未启用。'
	};

	state = {
		error: false,
		loading: false
	};

	componentDidMount() {
		this.props.dispatch({
			type: 'cloudServer/checkIDCardReady',
			payload: {},
			callBack: (data) => {
				if (!data || data[0].status != 200) {
					this.setState({ error: true });
					Modal.error({ content: this.props.errorText });
				}
			}
		});
	}

	render() {
		const { onRead, text, style, errorText } = this.props;
		const { error, loading } = this.state;
		return (
			<div style={style}>
				<Button
					disabled={error}
					loading={loading}
					type="primary"
					onClick={() => {
						this.setState({ loading: true });
						this.props.dispatch({
							type: 'cloudServer/getIDCard',
							payload: {},
							callBack: (data) => {
								this.setState({ loading: false });
								if (data.resultFlag != 0) {
									message.error(data.errorMsg);
									return;
								}
								data.resultContent.identityPic =
									'data:image/jpeg;base64,' + data.resultContent.identityPic;
								onRead(data.resultContent);
							}
						});
					}}
				>
					{text}
				</Button>
			</div>
		);
	}
}

ReadIDCard = connect((state) => {
	return {
		...state.cloudServer
	};
})(ReadIDCard);
export default ReadIDCard;
