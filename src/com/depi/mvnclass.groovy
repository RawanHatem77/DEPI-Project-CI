package com.depi

class mvnclass implements Serializable {

    def steps

    mvnclass(steps) {
        this.steps = steps
    }

    def packageJar(packagejavaOpt) {
        steps.sh " mvn clean package install ${packagejavaOpt} "
    }
    def testJar(testjavaOpt) {
        steps.sh " mvn clean package install ${testjavaOpt} "
    }

}

