import * as THREE from 'three'
import { UnworkableCamera } from './module/UnworkableCamera'
import type { RawShaderMaterial } from './module/ExtendedMaterials'

export abstract class SceneBase {
	protected readonly DETAIL = 4
	protected readonly SCALE = 6
	protected readonly MESH_NAME = 'screen'

	protected readonly scene: THREE.Scene
	protected readonly camera: UnworkableCamera

	constructor(protected readonly renderer: THREE.WebGLRenderer) {
		this.scene = new THREE.Scene()
		this.camera = new UnworkableCamera()
	}

	protected get size() {
		const w = window.innerWidth
		const h = window.innerHeight
		return { width: w, height: h, aspect: w / h, dpr: window.devicePixelRatio }
	}

	protected addMesh(mesh: THREE.Mesh) {
		mesh.name = this.MESH_NAME
		this.scene.add(mesh)
	}

	protected get mesh() {
		return this.scene.getObjectByName(this.MESH_NAME) as THREE.Mesh<THREE.PlaneGeometry, RawShaderMaterial>
	}

	protected get uniforms() {
		return this.mesh.material.uniforms
	}
}
