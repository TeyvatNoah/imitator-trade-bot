#!/usr/bin/env bash

# 默认参数值
rid=
exe=
arch=

# 解析命令行选项
while getopts ":r:e:a:" opt; do
	case $opt in
		r) rid=$OPTARG ;;
		e) exe=$OPTARG ;;
		a) arch=$OPTARG ;;
		\?)
			echo "无效的选项: -$OPTARG" >&2
			exit 1 ;;
	esac
done

if [[ $rid && $arch && $exe ]]; then
	echo 'Release Build'
	dotnet publish -c Release -p:Arch=$arch -p:RID=$rid -p:ExeFile=$exe
else
	echo 'Debug Build'
	dotnet publish -c Debug
fi