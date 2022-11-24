import Axios from 'axios'

const RESOURCE_NAME = 'https://localhost:44334/api/statistics'

export default {
  get (from, to, month) {
    return Axios.get(`${RESOURCE_NAME}&from=${from}&to=${to}&month=${month}`)
  },
  getAll () {
    return Axios.get(`${RESOURCE_NAME}`)
  },
  getChartData (type, from, to, month) {
    return Axios.get(`${RESOURCE_NAME}/chart?type=${type}&from=${from}&to=${to}&month=${month}`)
  }
}
