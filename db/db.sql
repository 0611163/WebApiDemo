/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50728
Source Host           : localhost:3306
Source Database       : litesql_test

Target Server Type    : MYSQL
Target Server Version : 50728
File Encoding         : 65001

Date: 2023-04-25 12:05:35
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `bs_order`
-- ----------------------------
DROP TABLE IF EXISTS `bs_order`;
CREATE TABLE `bs_order` (
  `id` varchar(50) NOT NULL COMMENT '主键',
  `order_time` datetime NOT NULL COMMENT '订单时间',
  `amount` decimal(20,2) DEFAULT NULL COMMENT '订单金额',
  `order_userid` bigint(20) NOT NULL COMMENT '下单用户',
  `status` tinyint(4) NOT NULL COMMENT '订单状态(0草稿 1已下单 2已付款 3已发货 4完成)',
  `remark` varchar(255) DEFAULT NULL COMMENT '备注',
  `create_userid` varchar(50) NOT NULL COMMENT '创建者ID',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `update_userid` varchar(50) DEFAULT NULL COMMENT '更新者ID',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='订单表';

-- ----------------------------
-- Records of bs_order
-- ----------------------------
BEGIN;
INSERT INTO `bs_order` VALUES ('100001', '2022-01-19 12:04:42', '1393.50', '10', '0', '订单已修改32', '10', '2022-01-19 12:04:42', '10', '2023-02-28 08:32:37'), ('100002', '2022-01-19 12:05:23', '17268.42', '10', '0', '测试订单002', '10', '2022-01-19 12:05:23', null, null), ('100003', '2022-01-19 12:05:59', '17268.42', '10', '0', '测试订单003', '10', '2022-01-19 12:05:59', null, null), ('5ce9560658c848058b9a958705e4e856', '2023-02-28 08:32:35', '17268.42', '10', '0', null, '10', '2023-02-28 08:32:35', null, null), ('8f0cf40fc4ee439aabef19099bcb06c1', '2023-02-28 08:32:35', '17268.42', '10', '0', null, '10', '2023-02-28 08:32:35', null, null), ('9e1981f74630476ea8b30e90ec542f7d', '2023-02-28 08:32:35', '17268.42', '10', '0', null, '10', '2023-02-28 08:32:35', null, null);
COMMIT;

-- ----------------------------
-- Table structure for `bs_order_detail`
-- ----------------------------
DROP TABLE IF EXISTS `bs_order_detail`;
CREATE TABLE `bs_order_detail` (
  `id` varchar(50) NOT NULL COMMENT '主键',
  `order_id` varchar(50) NOT NULL COMMENT '订单ID',
  `goods_name` varchar(200) NOT NULL COMMENT '商品名称',
  `quantity` int(11) NOT NULL COMMENT '数量',
  `price` decimal(20,2) NOT NULL COMMENT '价格',
  `spec` varchar(100) DEFAULT NULL COMMENT '物品规格',
  `order_num` int(11) DEFAULT NULL COMMENT '排序',
  `create_userid` varchar(50) NOT NULL COMMENT '创建者ID',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `update_userid` varchar(50) DEFAULT NULL COMMENT '更新者ID',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='订单明细表';

-- ----------------------------
-- Records of bs_order_detail
-- ----------------------------
BEGIN;
INSERT INTO `bs_order_detail` VALUES ('10000201', '100002', '键盘', '11', '123.66', '个', '3', '10', '2022-01-19 12:05:23', null, null), ('10000202', '100002', '鼠标', '12', '50.68', '个', '2', '10', '2022-01-19 12:05:23', null, null), ('10000203', '100002', '电脑', '3', '5100.00', '台', '1', '10', '2022-01-19 12:05:23', null, null), ('10000301', '100003', '鼠标', '12', '50.68', '个', '2', '10', '2022-01-19 12:05:59', null, null), ('10000302', '100003', '电脑', '3', '5100.00', '台', '1', '10', '2022-01-19 12:05:59', null, null), ('10000303', '100003', '键盘', '11', '123.66', '个', '3', '10', '2022-01-19 12:05:59', null, null), ('1e81643867df41feb62a0d0be305626c', '5ce9560658c848058b9a958705e4e856', '鼠标', '12', '50.68', '个', '2', '10', '2023-02-28 08:32:35', null, null), ('20cdd5da708f42d0870e52cb4639fe7b', '8f0cf40fc4ee439aabef19099bcb06c1', '鼠标', '12', '50.68', '个', '2', '10', '2023-02-28 08:32:35', null, null), ('324d79b7d5084a5eb7aae245824586ad', '8f0cf40fc4ee439aabef19099bcb06c1', '电脑', '3', '5100.00', '台', '1', '10', '2023-02-28 08:32:35', null, null), ('3e83a35c2bf04559bfc343a79c89bb45', '9e1981f74630476ea8b30e90ec542f7d', '鼠标', '12', '50.68', '个', '2', '10', '2023-02-28 08:32:35', null, null), ('5028ffcea67940e388f35f748d976162', '5ce9560658c848058b9a958705e4e856', '电脑', '3', '5100.00', '台', '1', '10', '2023-02-28 08:32:35', null, null), ('532155596ce94b59a0de91802b73076d', '5ce9560658c848058b9a958705e4e856', '键盘', '11', '123.66', '个', '3', '10', '2023-02-28 08:32:35', null, null), ('5515da4d56bc499382d5d38eef2c9371', '100001', '桌子32', '10', '78.89', '张', '4', '10', '2023-02-28 08:32:37', null, null), ('95c7ea7db3ef4d549f1277515e043413', '8f0cf40fc4ee439aabef19099bcb06c1', '键盘', '11', '123.66', '个', '3', '10', '2023-02-28 08:32:35', null, null), ('a48058e02e124026817fcaacd8687bdb', '9e1981f74630476ea8b30e90ec542f7d', '电脑', '3', '5100.00', '台', '1', '10', '2023-02-28 08:32:35', null, null), ('b75358a1c6d64e5daec6c927be804d35', '9e1981f74630476ea8b30e90ec542f7d', '键盘', '11', '123.66', '个', '3', '10', '2023-02-28 08:32:35', null, null), ('dfde40a0104e4450ac5ef35167452ef1', '100001', '椅子32', '20', '30.23', '把', '5', '10', '2023-02-28 08:32:37', null, null);
COMMIT;

-- ----------------------------
-- Table structure for `sys_user`
-- ----------------------------
DROP TABLE IF EXISTS `sys_user`;
CREATE TABLE `sys_user` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `user_name` varchar(50) NOT NULL COMMENT '用户名',
  `real_name` varchar(50) DEFAULT NULL COMMENT '用户姓名',
  `password` varchar(200) NOT NULL COMMENT '用户密码',
  `remark` varchar(200) DEFAULT NULL COMMENT '备注',
  `create_userid` varchar(50) NOT NULL COMMENT '创建者ID',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `update_userid` varchar(50) DEFAULT NULL COMMENT '更新者ID',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COMMENT='用户表';

-- ----------------------------
-- Records of sys_user
-- ----------------------------
BEGIN;
INSERT INTO `sys_user` VALUES ('1', 'admin', '超级管理员', '123456', '测试修改用户8002', '1', '2020-11-01 13:39:43', '1', '2022-10-22 19:32:40'), ('2', 'admin2020', '普通管理员', '123456', '测试修改用户1050', '1', '2020-11-01 13:42:55', '1', '2022-10-22 19:32:40'), ('9', 'wangwu', '王五', '123456', '测试修改用户1503', '1', '2022-01-19 12:10:17', null, null), ('10', 'zhangsan', '张三', '123456', '测试修改用户52', '1', '2020-11-01 13:40:30', '1', '2023-02-28 08:32:37'), ('11', 'lisi', '李四', '123456', '测试修改用户627', '1', '2020-11-01 13:42:08', '1', '2022-10-22 19:32:40'), ('12', 'testUser', '测试插入用户', '123456', '测试修改用户9992', '1', '2022-10-17 14:02:24', '1', '2022-10-22 19:32:40'), ('13', 'testUser', '测试插入用户', '123456', null, '1', '2023-02-28 08:32:31', null, null), ('14', 'testUser', '测试插入用户', '123456', null, '1', '2023-02-28 08:32:31', null, null), ('15', 'testUser', '测试插入用户', '123456', null, '1', '2023-02-28 08:32:31', null, null), ('16', 'testUser', '测试插入用户', '123456', null, '1', '2023-02-28 08:32:31', null, null), ('17', 'testUser', '测试插入用户', '123456', null, '1', '2023-02-28 08:32:31', null, null), ('18', 'testUser', '测试插入用户', '123456', null, '1', '2023-02-28 08:32:31', null, null), ('19', 'testUser', '测试插入用户', '123456', null, '1', '2023-02-28 08:32:31', null, null), ('20', 'testUser', '测试插入用户', '123456', null, '1', '2023-02-28 08:32:31', null, null);
COMMIT;

-- ----------------------------
-- Table structure for `sys_user_202208`
-- ----------------------------
DROP TABLE IF EXISTS `sys_user_202208`;
CREATE TABLE `sys_user_202208` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `user_name` varchar(50) NOT NULL COMMENT '用户名',
  `real_name` varchar(50) DEFAULT NULL COMMENT '用户姓名',
  `password` varchar(200) NOT NULL COMMENT '用户密码',
  `remark` varchar(200) DEFAULT NULL COMMENT '备注',
  `create_userid` varchar(50) NOT NULL COMMENT '创建者ID',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `update_userid` varchar(50) DEFAULT NULL COMMENT '更新者ID',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COMMENT='用户表';

-- ----------------------------
-- Records of sys_user_202208
-- ----------------------------
BEGIN;
INSERT INTO `sys_user_202208` VALUES ('1', 'admin', '超级管理员', '123456', '超级管理员', '1', '2020-11-01 13:39:43', '1', '2020-11-01 13:39:47'), ('2', 'admin2020', '普通管理员', '123456', '普通管理员', '1', '2020-11-01 13:42:55', '1', '2020-11-01 13:42:58'), ('9', 'wangwu', '王五', '123456', '测试修改用户02', '1', '2022-01-19 12:10:17', '1', '2022-08-01 16:18:15'), ('10', 'zhangsan', '张三', '123456', '测试修改分表数据96', '1', '2020-11-01 13:40:30', '1', '2023-02-28 08:32:37'), ('11', 'lisi', '李四', '123456', '测试修改用户03', '1', '2020-11-01 13:42:08', '1', '2022-01-17 10:29:55'), ('12', 'testUser', '测试插入分表数据', '123456', '测试插入分表数据', '1', '2022-08-01 20:38:22', null, null), ('13', 'testUser', '测试插入分表数据', '123456', '测试插入分表数据', '1', '2022-08-01 20:43:18', null, null), ('14', 'testUser', '测试插入分表数据', '123456', '测试插入分表数据', '1', '2022-08-01 20:51:12', null, null), ('15', 'testUser', '测试插入分表数据', '123456', '测试插入分表数据', '1', '2022-08-01 22:48:44', null, null), ('16', 'testUser', '测试插入分表数据', '123456', '测试插入分表数据', '1', '2022-08-04 12:12:06', null, null), ('17', 'testUser', '测试插入分表数据', '123456', '测试插入分表数据', '1', '2022-08-04 12:40:18', null, null), ('18', 'testUser', '测试插入分表数据', '123456', '测试插入分表数据', '1', '2022-08-04 12:46:31', null, null), ('19', 'testUser', '测试插入分表数据', '123456', '测试插入分表数据', '1', '2022-08-04 12:49:33', null, null), ('20', 'testUser', '测试插入分表数据', '123456', '测试插入分表数据', '1', '2022-08-04 13:21:02', null, null);
COMMIT;

-- ----------------------------
-- Table structure for `values_test`
-- ----------------------------
DROP TABLE IF EXISTS `values_test`;
CREATE TABLE `values_test` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `bytes_value` blob COMMENT '字节数组测试',
  `byte_value` tinyint(4) DEFAULT NULL COMMENT 'Byte测试',
  `guid_value` varchar(50) DEFAULT NULL COMMENT 'Guid测试',
  `char_value` char(1) DEFAULT NULL,
  `chars_valiue` char(20) DEFAULT NULL,
  `bool_value` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of values_test
-- ----------------------------
BEGIN;
INSERT INTO `values_test` VALUES ('1', 0xE5AD97E6AEB5E7B1BBE59E8BE6B58BE8AF95, '123', '181d4961-2b35-4a18-96a7-9334740160ba', 'A', 'ABC', '0'), ('2', null, null, null, null, null, null), ('3', null, null, null, null, '', null), ('4', null, null, null, null, '', null), ('5', 0xE5AD97E6AEB5E7B1BBE59E8BE6B58BE8AF95, '123', 'd2a29c17-715d-4c87-afdc-a7db2ae10a2b', 'A', 'ABC', '1'), ('6', 0xE5AD97E6AEB5E7B1BBE59E8BE6B58BE8AF95, '123', 'ea22e74b-8935-4170-b174-56b4aae065bc', 'A', 'ABC', '0'), ('7', 0xE5AD97E6AEB5E7B1BBE59E8BE6B58BE8AF95, '123', 'e417d0ad-0199-405d-90c8-1450f4f8b54b', 'A', 'ABC', '1'), ('8', 0xE5AD97E6AEB5E7B1BBE59E8BE6B58BE8AF95, '123', '0d10e8d8-0268-4442-90b2-23c6b533461c', 'A', 'ABC', '0'), ('9', 0xE5AD97E6AEB5E7B1BBE59E8BE6B58BE8AF95, '123', 'fe90711d-c903-499c-a3e9-41e69929c183', 'A', 'ABC', '1'), ('10', 0xE5AD97E6AEB5E7B1BBE59E8BE6B58BE8AF95E4BFAEE694B9, '123', '68e4440f-f152-4b39-b246-f165f8f94f4c', 'B', 'DEF', '1');
COMMIT;
