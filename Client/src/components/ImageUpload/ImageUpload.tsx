import React from 'react';
import { Upload, message, Button, Icon, Modal, Tooltip } from 'antd';
import { remoteUrl } from './../../utils/url';
import ServerImageBrowse from './ServerImageBrowse';

class ImageUpload extends React.Component {
	static defaultProps = {
		action: `${remoteUrl}/ImageFile/UploadPicture`,
		serverUploadVisible: true,
		urlPrefix: ''
	};
	state = {
		loading: false,
		previewVisible: false,
		isHover: false,
		imageBrowseVisible: false
	};
	handlePreviewCancel = () => this.setState({ previewVisible: false });
	handlePreviewShow = () => {
		this.setState({
			previewVisible: true
		});
	};
	handleImageBrowseCancel = () => this.setState({ imageBrowseVisible: false });
	handleImageBrowseShow = () => {
		this.setState({
			imageBrowseVisible: true
		});
	};
	handleImageBrowseChange = (imageUrl) => {
		this.setState({
			imageBrowseVisible: false,
			isHover: false
		});
		this.props.onChange(imageUrl);
	};
	onMouseEnter = () => {
		this.setState({ isHover: true });
	};
	onMouseLeave = () => {
		this.setState({ isHover: false });
	};
	render() {
		let _this = this;

		const props = {
			name: 'file',
			action: this.props.action,
			showUploadList: false,
			beforeUpload(file) {
				var isTypeTrue = file.type === 'image/jpeg' || file.type === 'image/png' || file.type === 'image/gif';
				if (!isTypeTrue) {
					message.warning('请上传JPG/PNG/GIF类型的文件');
				}
				return isTypeTrue;
			},
			onChange(info) {
				if (info.file.status === 'uploading') {
					_this.setState({ loading: true });
					return;
				}
				_this.setState({
					loading: false
				});
				if (info.file.status === 'done') {
					if (info.fileList[info.fileList.length - 1].response.error) {
						message.error('上传失败' + info.fileList[info.fileList.length - 1].response.error.message);
						return;
					}
					let result = info.fileList[info.fileList.length - 1].response;
					let imageUrl;
					if (result.result && result.result.url) {
						imageUrl = result.result.url;
					} else {
						imageUrl = result;
					}
					_this.props.onChange(imageUrl);
					message.success('上传成功');
				} else if (info.file.status === 'error') {
					message.error('上传失败');
				}
			}
		};

		let previewImage = this.props.value ? this.props.urlPrefix + this.props.value : null;

		return (
			<div>
				<div>
					{previewImage ? (
						<img
							onMouseEnter={this.onMouseEnter}
							src={remoteUrl + previewImage}
							style={{
								width: '130px',
								height: '130px'
							}}
						/>
					) : (
						<div
							style={{
								width: '130px',
								height: '130px',
								lineHeight: '145px',
								border: '1px solid #d9d9d9',
								borderRadius: '4px'
							}}
						>
							<center>
								<Upload {...props}>
									<Icon
										style={{
											fontSize: 30,
											color: '#c6c8cb',
											cursor: 'pointer'
										}}
										type="upload"
									/>
								</Upload>
								{this.props.serverUploadVisible && (
									<Icon
										style={{
											fontSize: 30,
											color: '#c6c8cb',
											cursor: 'pointer',
											marginLeft: '12px'
										}}
										type="cloud-upload-o"
										onClick={this.handleImageBrowseShow}
									/>
								)}
							</center>
						</div>
					)}
				</div>
				<div
					onMouseLeave={this.onMouseLeave}
					style={{
						position: 'absolute',
						width: '130px',
						height: '130px',
						lineHeight: '145px',
						backgroundColor: 'rgba(0, 0, 0, 0.65)',
						top: 0,
						display: previewImage && this.state.isHover ? 'block' : 'none'
					}}
				>
					<center>
						<Icon
							style={{
								fontSize: 30,
								color: 'rgba(255, 255, 255, 0.85)',
								cursor: 'pointer',
								marginRight: '12px'
							}}
							type="eye-o"
							onClick={this.handlePreviewShow}
						/>
						<Upload {...props}>
							<Icon
								style={{
									fontSize: 30,
									color: 'rgba(255, 255, 255, 0.85)',
									cursor: 'pointer'
								}}
								type="upload"
							/>
						</Upload>
						{this.props.serverUploadVisible && (
							<Icon
								style={{
									fontSize: 30,
									color: 'rgba(255, 255, 255, 0.85)',
									cursor: 'pointer',
									marginLeft: '12px'
								}}
								type="cloud-upload-o"
								onClick={this.handleImageBrowseShow}
							/>
						)}
					</center>
				</div>

				<Modal visible={this.state.previewVisible} footer={null} onCancel={this.handlePreviewCancel}>
					<img style={{ width: '100%' }} src={remoteUrl + previewImage} />
				</Modal>

				<ServerImageBrowse
					onChange={this.handleImageBrowseChange}
					visible={this.state.imageBrowseVisible}
					onCancel={this.handleImageBrowseCancel}
				/>
			</div>
		);
	}
}

export default ImageUpload;
