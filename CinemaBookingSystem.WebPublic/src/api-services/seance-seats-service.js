import Axios from 'axios'

const RESOURCE_NAME = '/seance-seats'

export default {
  getDetail (id) {
    return Axios.get(`${RESOURCE_NAME}/detail/${id}`)
  },
  getBySeanceId (id) {
    return Axios.get(`${RESOURCE_NAME}/${id}`)
  },
  create (data) {
    return Axios.post(RESOURCE_NAME, data)
  },
  update (id, data) {
    return Axios.put(`${RESOURCE_NAME}/${id}`, data)
  },
  delete (id) {
    return Axios.delete(`${RESOURCE_NAME}/${id}`)
  },
  lock (data) {
    return Axios.post(`${RESOURCE_NAME}/seat-lock`, data)
  }
}
