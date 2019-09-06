//校验方法写在这里
//手机号码
export function validatemobile(mobile = '') {
  if (mobile.length == 0) {
    return false;
  }
  if (mobile.length != 11) {
    return false;
  }
  let myreg = new RegExp(/^\d{11}$/);
  if (!myreg.test(mobile)) {
    return false;
  }
  return true;
}

//校验6位验证码
export function validatecode(code = '') {
  if (code.length == 0) {
    return false;
  }
  if (code.length != 6) {
    return false;
  }
  let myreg = new RegExp(/^\d{6}$/);
  if (!myreg.test(code)) {
    return false;
  }
  return true;
}

//校验银行卡号
export function valibankcode(code = '') {
  if (code.length == 0) {
    return false;
  }
  let myreg = new RegExp(/^\d{16,20}$/);
  if (!myreg.test(code)) {
    return false;
  }
  return true;
}

//校验金额，两位小数
export function valiMoney(code = '') {
  code += '';
  if (code.length == 0) {
    return false;
  }
  let myreg = RegExp(/^[0-9]+(.[0-9]{1,2})?$/);
  if (!myreg.test(code)) {
    return false;
  }
  //超过5万
  if (parseFloat(code) > 50000) {
    return false
  }
  return true;
}

//校验金额， 大于1小余2000
export function valiAlipay(code = '') {
  if (parseFloat(code) > 2000) {
    return false;
  }
  ;
  if (parseFloat(code) < 1) {
    return false;
  }
  ;
  return true;
}

//校验金额， 大于0.01小余30000
export function valiWechatpay(code = '') {
  if (parseFloat(code) > 30000) {
    return false;
  }
  ;
  if (parseFloat(code) < 0.01) {
    return false;
  }
  ;
  return true;
}

//校验空
export function valiEnpty(code = '') {
  if (!code) {
    return false;
  }
  code = code.split('');
  if (code.length == 0) {
    return false;
  }
  return true;
}

//身份证号合法性验证
//支持15位和18位身份证号
//支持地址编码、出生日期、校验位验证
export function IdentityCodeValid(code = '') {
  let city = {
    11: "北京",
    12: "天津",
    13: "河北",
    14: "山西",
    15: "内蒙古",
    21: "辽宁",
    22: "吉林",
    23: "黑龙江 ",
    31: "上海",
    32: "江苏",
    33: "浙江",
    34: "安徽",
    35: "福建",
    36: "江西",
    37: "山东",
    41: "河南",
    42: "湖北 ",
    43: "湖南",
    44: "广东",
    45: "广西",
    46: "海南",
    50: "重庆",
    51: "四川",
    52: "贵州",
    53: "云南",
    54: "西藏 ",
    61: "陕西",
    62: "甘肃",
    63: "青海",
    64: "宁夏",
    65: "新疆",
    71: "台湾",
    81: "香港",
    82: "澳门",
    91: "国外 ",
  };

  let info = {
    tip: '',
    pass: true,
  };
  if (!code || !/(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/.test(code)) {
    info.tip = "身份证号格式错误";
    info.pass = false;
    return info;
  } else if (!city[code.substr(0, 2)]) {
    info.tip = "地址编码错误";
    info.pass = false;
    return info;
  } else {
    //18位身份证需要验证最后一位校验位
    if (code.length == 18) {
      code = code.toUpperCase();
      code = code.split('');
      //∑(ai×Wi)(mod 11)
      //加权因子
      let factor = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2];
      //校验位
      let parity = [1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2];
      let sum = 0;
      let ai = 0;
      let wi = 0;
      for (let i = 0; i < 17; i++) {
        ai = code[i];
        wi = factor[i];
        sum += ai * wi;
      }
      // let last = parity[sum % 11];
      if (parity[sum % 11] != code[17]) {
        info.tip = "校验位错误";
        info.pass = false;
        return info;
      }
    }
  }
  return info;
}
