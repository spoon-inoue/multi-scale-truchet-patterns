import * as THREE from 'three'
import { RawShaderMaterial } from '../module/ExtendedMaterials'
import { SceneBase } from '../SceneBase'
import fragmentShader from './shader/layerMap.fs'
import vertexShader from './shader/layerMap.vs'

export class LayerMap extends SceneBase {
	private readonly renderTarget: THREE.WebGLRenderTarget

	constructor(renderer: THREE.WebGLRenderer) {
		super(renderer)

		this.renderTarget = this.createRenderTarget()
		this.createPlane()
	}

	private createRenderTarget() {
		const size = Math.max(this.size.width, this.size.height)

		return new THREE.WebGLRenderTarget(size, size, {
			type: THREE.HalfFloatType,
			format: THREE.RedFormat,
			colorSpace: THREE.NoColorSpace,
			magFilter: THREE.NearestFilter,
			minFilter: THREE.NearestFilter,
			wrapS: THREE.RepeatWrapping,
			wrapT: THREE.RepeatWrapping,
		})
	}

	private createPlane() {
		let fs = fragmentShader
		fs = fs.replaceAll('DETAIL', this.DETAIL.toString())
		fs = fs.replaceAll('SCALE', this.SCALE.toString())

		const geo = new THREE.PlaneGeometry(2, 2)
		const mat = new RawShaderMaterial({
			uniforms: {
				resolution: { value: [this.size.width, this.size.height, this.size.dpr] },
				time: { value: 0 },
			},
			vertexShader,
			fragmentShader: fs,
		})
		const mesh = new THREE.Mesh(geo, mat)
		this.addMesh(mesh)
	}

	get texture() {
		return this.renderTarget.texture
	}

	resize() {
		const { width, height, dpr } = this.size
		const size = Math.max(this.size.width, this.size.height)

		this.renderTarget.setSize(size, size)
		this.uniforms.resolution.value = [width, height, dpr]
	}

	render(dt: number) {
		this.uniforms.time.value += dt
		this.renderer.setRenderTarget(this.renderTarget)
		this.renderer.render(this.scene, this.camera)
	}
}
