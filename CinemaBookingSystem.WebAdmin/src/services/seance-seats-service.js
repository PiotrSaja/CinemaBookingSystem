import Axios from 'axios'

const RESOURCE_NAME = 'https://saja.website:44351/api/seance-seats'

export default {
  getAll (seanceId) {
    return Axios.get(`${RESOURCE_NAME}/${seanceId}`)
  },
  get (id) {
    return Axios.get(`${RESOURCE_NAME}/detail/${id}`)
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
  createSeats (data) {
    return Axios.post(`${RESOURCE_NAME}/list`, data)
  },
}
