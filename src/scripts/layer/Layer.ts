import * as THREE from 'three'
import { RawShaderMaterial } from '../module/ExtendedMaterials'
import { SceneBase } from '../SceneBase'
import fragmentShader from './shader/layer.fs'
import vertexShader from './shader/layer.vs'

export class Layer extends SceneBase {
	constructor(renderer: THREE.WebGLRenderer, layerMap: THREE.Texture) {
		super(renderer)
		this.create(layerMap)
	}

	private create(layerMap: THREE.Texture) {
		let fs = fragmentShader
		fs = fs.replaceAll('DETAIL', this.DETAIL.toString())
		fs = fs.replaceAll('SCALE', this.SCALE.toString())

		const geo = new THREE.PlaneGeometry(2, 2)
		const mat = new RawShaderMaterial({
			uniforms: {
				layerMap: { value: layerMap },
				resolution: { value: [this.size.width, this.size.height, this.size.dpr] },
				time: { value: 0 },
			},
			vertexShader,
			fragmentShader: fs,
		})
		const mesh = new THREE.Mesh(geo, mat)
		this.addMesh(mesh)
	}

	resize() {
		const { width, height, dpr } = this.size
		this.uniforms.resolution.value = [width, height, dpr]
	}

	render(dt: number) {
		this.uniforms.time.value += dt
		this.renderer.setRenderTarget(null)
		this.renderer.render(this.scene, this.camera)
	}
}
