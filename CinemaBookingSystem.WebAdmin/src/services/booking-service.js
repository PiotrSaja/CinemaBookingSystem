import Axios from 'axios'

const RESOURCE_NAME = 'https://saja.website:44351/api/bookings'

export default {
  getAll (page, limit) {
    return Axios.get(`${RESOURCE_NAME}?page=${page}&limit=${limit}`)
  },
  get (id) {
    return Axios.get(`${RESOURCE_NAME}/${id}`)
  },
  create (data) {
    return Axios.post(RESOURCE_NAME, data)
  },
  update (id, data) {
    return Axios.put(`${RESOURCE_NAME}/${id}`, data)
  },
  delete (id) {
    return Axios.delete(`${RESOURCE_NAME}?id=${id}`)
  }
}
