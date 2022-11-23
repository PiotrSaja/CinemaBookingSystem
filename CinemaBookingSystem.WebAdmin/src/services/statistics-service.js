import Axios from 'axios'

const RESOURCE_NAME = 'https://saja.website:44351/api/statistics'

export default {
  get (from, to, month) {
    return Axios.get(`${RESOURCE_NAME}&from=${from}&to=${to}&month=${month}`)
  },
  getChartData (type, from, to, month) {
    return Axios.get(`${RESOURCE_NAME}/chart?type=${type}&from=${from}&to=${to}&month=${month}`)
  }
}
