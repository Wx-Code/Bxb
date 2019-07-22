const { export_json_to_excel } = require('src/utils/Export2Excel') //这里必须使用绝对路径
const XLSX = require('xlsx')

export default {
  /**
   * 将 json 导出为 Excel 文件
   * @param {Object|Array} data 数据源
   * @param {Object} fieldMap 字段名称和头部对应关系
   * @param {String} filename 下载文件名称
   * @example
   *        exportExcel(
    {
      'wId': '提现单号',
      'userId': '用户ID',
      'amount': '提现金额',
      'serviceAmount': '手续费',
      'bankCardAccount': '提现账户姓名',
      'bankCard': '银行卡号',
      'bankName': '银行名称',
      'createTime': '申请时间',
      'stateName': '状态',
      'auditTime': '审核时间',
    },
    this.excelData,
    '提现导出记录'
  )
  */
  export(data, fieldMap, filename) {
    require.ensure([], () => {
      if (!Array.isArray(data[0])) {
        data = [data]
      }

      // 提取表头和字段名称
      const headers = []
      const fieldNames = []
      for (const k in fieldMap) {
        if (fieldMap.hasOwnProperty(k)) {
          headers.push(fieldMap[k])
          fieldNames.push(k)
        }
      }

      // 处理数据格式
      const list = data.map((value, index) => {
        const sheetName = value.sheetName || `Sheet${index + 1}`
        const json = value.json || value
        return { sheetName, json }
      })
      console.log('[info] list', list)
      // 格式化 json
      const formattedData = list.map(v => {
        const array = Array.isArray(v.json) ? v.json : [v.json]
        return {
          sheetName: v.sheetName,
          jsonData: array.map(item => fieldNames.map(n => item[n])),
          th: headers
        }
      })

      export_json_to_excel(formattedData, filename)
    })
  },

  /**
   * 从 excel 文件中提取 json 数据并返回
   * @param {File} file 导入的文件
   * @param {Object} colMap 表头与字段名称 map
   *  如：{
   *    '物流编号': 'logisticsNumber',
   *    '物流公司': 'logisticsCompany',
   *  }
   */
  import(file, colMap) {
    return new Promise((resolve, reject) => {
      try {
        const onload = function(e) {
          const data = e.target.result
          // const bytes = new Uint8Array(data)
          // const byteString = bytes.map(b => String.fromCharCode(b)).join('')
          const workbook = XLSX.read(data, { type: 'binary' })
          const firstSheetName = workbook.SheetNames[0]
          const sheet = workbook.Sheets[firstSheetName]
          const sheetJson = XLSX.utils.sheet_to_json(sheet)

          const headers = Object.keys(colMap)
          const result = sheetJson.map(row => {
            const o = {}
            headers.forEach(h => {
              const fieldName = colMap[h]
              o[fieldName] = row[h]
            })
            return o
          })

          resolve(result)
        }

        const reader = new FileReader()
        reader.onload = onload
        reader.readAsBinaryString(file)
      } catch (err) {
        reject(err)
      }
    })
  }
}
