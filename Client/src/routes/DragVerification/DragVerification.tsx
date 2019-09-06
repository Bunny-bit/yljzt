import React from 'react';
import styles from './DragVerification.css';
import { Form, Icon } from 'antd';
const create = Form.create;
import { connect } from 'dva';

class DragVerification extends React.Component {
	state = {
		isMove: false,
		x: 0,
		startX: -1,
		arrayDate: [],
		startDate: new Date()
	};

	componentDidMount() {
		this.props.dispatch({
			type: 'dragVerification/getDragVerificationCode'
		});
	}

	refeshHandle = () => {
		this.props.dispatch({
			type: 'dragVerification/getDragVerificationCode'
		});
	};

	MouseDownHandle = (e) => {
		this.setState({
			...this.state,
			isMove: true,
			x: 0,
			startX: -1,
			arrayDate: [],
			startDate: new Date()
		});
		document.onmousemove = this.MouseMoveHandle;
		document.onmouseup = this.MouseUpHandle;
	};
	MouseMoveHandle = (e) => {
		if (this.state.startX < 0) {
			this.state.startX = e.pageX;
		} else {
			let tempX = e.pageX - this.state.startX;
			if (tempX >= 0 && tempX <= this.props.imgX - 40) {
				this.setState({
					...this.state,
					x: tempX,
					arrayDate: [ ...this.state.arrayDate, [ tempX, new Date().getTime() ] ]
				});
			}
		}
	};
	MouseUpHandle = (e) => {
		document.onmousemove = null;
		document.onmouseup = null;
		var payload = {
			point: this.state.x,
			timespan: new Date() - this.state.startDate,
			datelist: this.state.arrayDate.join('|'),
			callback: (value) => {
				this.props.onChange(value);
			}
		};
		this.setState({
			...this.state,
			isMove: false,
			x: 0,
			startX: -1
		});
		this.props.dispatch({
			type: 'dragVerification/checkCode',
			payload: payload
		});
	};

	render() {
		const { y, array, imgX, imgY, small, normal, success, myX } = this.props;
		let cutX = imgX / 10;
		let cutY = imgY / 2;
		const cutStyle = {
			float: 'left',
			margin: '0 !important',
			border: '0px',
			padding: '0 !important',
			width: cutX,
			height: cutY,
			backgroundImage: 'url(' + normal + ')'
		};
		return (
			<div className={styles.content}>
				<div className={styles.img}>
					{array.map((n, i) => {
						var num = array.indexOf(i); //第i张图相对于混淆图片的位置为num
						let x = 0;
						let y = 0;
						//还原前偏移
						y = i > 9 ? -cutY : 0;
						x = i > 9 ? (i - 10) * -cutX : i * -cutX;
						//当前y轴偏移量
						if (num > 9 && i < 10) y = y - cutY;
						if (i > 9 && num < 10) y = y + cutY;
						//当前x轴偏移量
						x = x + (num - i) * -cutX;
						return <div key={i} style={{ ...cutStyle, backgroundPosition: x + 'px ' + y + 'px' }} />;
					})}
				</div>
				<div
					className={styles.dragImg}
					style={{
						backgroundImage: 'url(' + small + ')',
						top: y,
						left: success ? myX : this.state.x
					}}
				/>
				<div className={styles.bord}>
					<div
						className={styles.dragBack}
						style={{
							width: success ? '100%' : this.state.x
						}}
					/>
					<div className={styles.dragText}>{success ? '验证通过' : '拖动图片验证'}</div>
					{success ? null : (
						<div
							className={styles.handler}
							onMouseDown={this.MouseDownHandle}
							style={{
								left: this.state.x
							}}
						>
							<Icon type="double-right" />
						</div>
					)}
					{success ? (
						<div
							className={styles.handler}
							style={{
								right: 0,
								left: 'initial'
							}}
						>
							<Icon type="like" />
						</div>
					) : (
						<a className={styles.refesh} title="点击刷新验证码" onClick={this.refeshHandle}>
							<Icon type="sync" />
						</a>
					)}
				</div>
			</div>
		);
	}
}

DragVerification = Form.create()(DragVerification);

export default connect((state) => {
	return {
		...state.dragVerification
	};
})(DragVerification);
