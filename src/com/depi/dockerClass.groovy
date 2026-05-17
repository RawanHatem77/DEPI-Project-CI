package com.depi

class dockerClass implements Serializable {

    def steps

    dockerClass(steps) {
        this.steps = steps
    }
    def dockerBuild(imageName , imageTag) {
        steps.sh " docker build -t ${imageName}:${imageTag} ."
    }

}
